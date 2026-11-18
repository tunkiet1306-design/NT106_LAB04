using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LAB04.Bai07;

/// <summary>
/// Embedded lightweight REST API server using <see cref="HttpListener"/> so Bài 07
/// runs immediately out-of-the-box without external dependencies or API keys.
/// </summary>
public sealed class LocalFoodApiServer : IDisposable
{
    public const string DefaultBaseUrl = "http://localhost:5077";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly string _prefix;
    private readonly string _storePath;
    private readonly object _syncLock = new();
    private readonly Dictionary<string, string> _activeTokens = new();
    private HttpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _listenTask;
    private StoreData _store;

    public LocalFoodApiServer(string prefix = "http://localhost:5077/", string? storePath = null)
    {
        _prefix = prefix.EndsWith('/') ? prefix : prefix + "/";
        _storePath = storePath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LAB04_Bai07",
            "store.json");
        _store = LoadOrCreateStore();
    }

    public bool IsRunning { get; private set; }

    public void Start()
    {
        if (IsRunning)
        {
            return;
        }

        _listener = new HttpListener();
        _listener.Prefixes.Add(_prefix);
        _listener.Start();

        _cts = new CancellationTokenSource();
        IsRunning = true;
        _listenTask = Task.Run(() => ListenLoopAsync(_cts.Token));
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }

        IsRunning = false;
        _cts?.Cancel();
        try
        {
            _listener?.Stop();
            _listener?.Close();
        }
        catch
        {
            // Ignore shutdown exceptions from HttpListener.
        }
    }

    public void Dispose()
    {
        Stop();
        _cts?.Dispose();
    }

    private async Task ListenLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested && _listener is { IsListening: true })
        {
            HttpListenerContext? context = null;
            try
            {
                context = await _listener.GetContextAsync();
                _ = Task.Run(() => HandleRequestAsync(context), ct);
            }
            catch (HttpListenerException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch
            {
                context?.Response.Close();
            }
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        var req = context.Request;
        var res = context.Response;
        res.ContentType = "application/json; charset=utf-8";

        try
        {
            var path = req.Url?.AbsolutePath.TrimEnd('/') ?? string.Empty;
            var method = req.HttpMethod.ToUpperInvariant();

            if (method == "POST" && path == "/auth/token")
            {
                await HandleTokenAsync(req, res);
                return;
            }

            if (method == "POST" && path == "/api/v1/user/signup")
            {
                await HandleSignUpAsync(req, res);
                return;
            }

            if (method == "GET" && path == "/api/v1/user/me")
            {
                await HandleMeAsync(req, res);
                return;
            }

            if (method == "POST" && path == "/api/v1/monan/all")
            {
                await HandleGetDishesAsync(req, res, onlyMine: false);
                return;
            }

            if (method == "POST" && path == "/api/v1/monan/my-dishes")
            {
                await HandleGetDishesAsync(req, res, onlyMine: true);
                return;
            }

            if (method == "POST" && path == "/api/v1/monan/add")
            {
                await HandleAddDishAsync(req, res);
                return;
            }

            if (method == "DELETE" && path.StartsWith("/api/v1/monan/", StringComparison.Ordinal))
            {
                var id = path["/api/v1/monan/".Length..];
                await HandleDeleteDishAsync(req, res, id);
                return;
            }

            await WriteJsonAsync(res, HttpStatusCode.NotFound, new { detail = "Endpoint not found" });
        }
        catch (Exception ex)
        {
            await WriteJsonAsync(res, HttpStatusCode.InternalServerError, new { detail = ex.Message });
        }
    }

    private async Task HandleTokenAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        var body = await ReadBodyStringAsync(req);
        var (username, password) = ParseCredentials(req.ContentType, body);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await WriteJsonAsync(res, HttpStatusCode.BadRequest, new { detail = "Username and password are required" });
            return;
        }

        string token;
        lock (_syncLock)
        {
            var user = _store.Users.FirstOrDefault(u =>
                string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) &&
                u.PasswordHash == HashPassword(password));

            if (user is null)
            {
                token = string.Empty;
            }
            else
            {
                token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
                _activeTokens[token] = user.Username;
            }
        }

        if (string.IsNullOrEmpty(token))
        {
            await WriteJsonAsync(res, HttpStatusCode.Unauthorized, new { detail = "Sai tên đăng nhập hoặc mật khẩu." });
            return;
        }

        await WriteJsonAsync(res, HttpStatusCode.OK, new TokenResponse
        {
            TokenType = "Bearer",
            AccessToken = token
        });
    }

    private async Task HandleSignUpAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        var body = await ReadBodyStringAsync(req);
        var signup = JsonSerializer.Deserialize<SignUpRequest>(body, JsonOptions);
        if (signup is null || string.IsNullOrWhiteSpace(signup.Username) || string.IsNullOrWhiteSpace(signup.Password))
        {
            await WriteJsonAsync(res, HttpStatusCode.BadRequest, new { detail = "Vui lòng nhập đầy đủ Username và Password." });
            return;
        }

        UserRecord created;
        lock (_syncLock)
        {
            if (_store.Users.Any(u => string.Equals(u.Username, signup.Username.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                created = null!;
            }
            else
            {
                created = new UserRecord
                {
                    Id = Guid.NewGuid().ToString("N")[..8],
                    Username = signup.Username.Trim(),
                    PasswordHash = HashPassword(signup.Password),
                    Email = signup.Email.Trim(),
                    FirstName = signup.FirstName.Trim(),
                    LastName = signup.LastName.Trim(),
                    Phone = signup.Phone.Trim(),
                    Birthday = signup.Birthday.Trim(),
                    Language = string.IsNullOrWhiteSpace(signup.Language) ? "vi" : signup.Language.Trim(),
                    Sex = signup.Sex
                };
                _store.Users.Add(created);
                SaveStore();
            }
        }

        if (created is null)
        {
            await WriteJsonAsync(res, HttpStatusCode.BadRequest, new { detail = "Tên tài khoản đã tồn tại." });
            return;
        }

        await WriteJsonAsync(res, HttpStatusCode.Created, created.ToPublic());
    }

    private async Task HandleMeAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        var username = Authenticate(req);
        if (username is null)
        {
            await WriteJsonAsync(res, HttpStatusCode.Unauthorized, new { detail = "Chưa đăng nhập hoặc token không hợp lệ." });
            return;
        }

        UserInfo? info;
        lock (_syncLock)
        {
            info = _store.Users
                .FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase))
                ?.ToPublic();
        }

        if (info is null)
        {
            await WriteJsonAsync(res, HttpStatusCode.NotFound, new { detail = "Không tìm thấy người dùng." });
            return;
        }

        await WriteJsonAsync(res, HttpStatusCode.OK, info);
    }

    private async Task HandleGetDishesAsync(HttpListenerRequest req, HttpListenerResponse res, bool onlyMine)
    {
        string? username = null;
        if (onlyMine)
        {
            username = Authenticate(req);
            if (username is null)
            {
                await WriteJsonAsync(res, HttpStatusCode.Unauthorized, new { detail = "Vui lòng đăng nhập để xem món ăn bạn đóng góp." });
                return;
            }
        }

        var body = await ReadBodyStringAsync(req);
        var pageReq = string.IsNullOrWhiteSpace(body)
            ? new PaginationRequest()
            : JsonSerializer.Deserialize<PaginationRequest>(body, JsonOptions) ?? new PaginationRequest();

        var current = Math.Max(1, pageReq.Current);
        var pageSize = Math.Clamp(pageReq.PageSize, 1, 50);

        DishPageResponse result;
        lock (_syncLock)
        {
            IEnumerable<DishItem> query = _store.Dishes;
            if (onlyMine && username is not null)
            {
                query = query.Where(d => string.Equals(d.NguoiDongGop, username, StringComparison.OrdinalIgnoreCase));
            }

            var list = query.ToList();
            var total = list.Count;
            var items = list
                .Skip((current - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            result = new DishPageResponse
            {
                Data = items,
                Pagination = new PaginationMeta
                {
                    Current = current,
                    PageSize = pageSize,
                    Total = total
                }
            };
        }

        await WriteJsonAsync(res, HttpStatusCode.OK, result);
    }

    private async Task HandleAddDishAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        var username = Authenticate(req);
        if (username is null)
        {
            await WriteJsonAsync(res, HttpStatusCode.Unauthorized, new { detail = "Vui lòng đăng nhập để thêm món ăn." });
            return;
        }

        var body = await ReadBodyStringAsync(req);
        var input = JsonSerializer.Deserialize<CreateDishRequest>(body, JsonOptions);
        if (input is null || string.IsNullOrWhiteSpace(input.TenMonAn))
        {
            await WriteJsonAsync(res, HttpStatusCode.BadRequest, new { detail = "Tên món ăn không được để trống." });
            return;
        }

        DishItem created;
        lock (_syncLock)
        {
            created = new DishItem
            {
                Id = Guid.NewGuid().ToString("N")[..8],
                TenMonAn = input.TenMonAn.Trim(),
                Gia = input.Gia,
                DiaChi = input.DiaChi.Trim(),
                HinhAnh = input.HinhAnh.Trim(),
                MoTa = input.MoTa.Trim(),
                NguoiDongGop = username
            };
            _store.Dishes.Insert(0, created);
            SaveStore();
        }

        await WriteJsonAsync(res, HttpStatusCode.Created, created);
    }

    private async Task HandleDeleteDishAsync(HttpListenerRequest req, HttpListenerResponse res, string id)
    {
        var username = Authenticate(req);
        if (username is null)
        {
            await WriteJsonAsync(res, HttpStatusCode.Unauthorized, new { detail = "Vui lòng đăng nhập để xóa món ăn." });
            return;
        }

        int statusCode;
        string message;
        lock (_syncLock)
        {
            var dish = _store.Dishes.FirstOrDefault(d => string.Equals(d.Id, id, StringComparison.OrdinalIgnoreCase));
            if (dish is null)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Không tìm thấy món ăn.";
            }
            else if (!string.Equals(dish.NguoiDongGop, username, StringComparison.OrdinalIgnoreCase))
            {
                statusCode = (int)HttpStatusCode.Forbidden;
                message = "Bạn chỉ có thể xóa món ăn do chính mình đóng góp.";
            }
            else
            {
                _store.Dishes.Remove(dish);
                SaveStore();
                statusCode = (int)HttpStatusCode.OK;
                message = "Đã xóa món ăn.";
            }
        }

        await WriteJsonAsync(res, (HttpStatusCode)statusCode, new { detail = message });
    }

    private string? Authenticate(HttpListenerRequest req)
    {
        var auth = req.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var token = auth["Bearer ".Length..].Trim();
        lock (_syncLock)
        {
            return _activeTokens.TryGetValue(token, out var username) ? username : null;
        }
    }

    private static (string Username, string Password) ParseCredentials(string? contentType, string body)
    {
        if (!string.IsNullOrWhiteSpace(contentType) && contentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
        {
            var boundaryIndex = contentType.IndexOf("boundary=", StringComparison.OrdinalIgnoreCase);
            if (boundaryIndex >= 0)
            {
                var boundary = contentType[(boundaryIndex + "boundary=".Length)..].Trim().Trim('"');
                var parts = body.Split("--" + boundary, StringSplitOptions.RemoveEmptyEntries);
                var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var part in parts)
                {
                    if (part.Trim() == "--") continue;
                    var sep = part.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                    if (sep < 0) continue;
                    var header = part[..sep];
                    var val = part[(sep + 4)..].TrimEnd('\r', '\n');
                    var nameMarker = "name=";
                    var idx = header.IndexOf(nameMarker, StringComparison.OrdinalIgnoreCase);
                    if (idx < 0) continue;
                    var afterName = header[(idx + nameMarker.Length)..].TrimStart();
                    string name;
                    if (afterName.StartsWith('"'))
                    {
                        var endQuote = afterName.IndexOf('"', 1);
                        if (endQuote < 0) continue;
                        name = afterName[1..endQuote];
                    }
                    else
                    {
                        var endIdx = afterName.IndexOfAny([';', '\r', '\n']);
                        name = (endIdx < 0 ? afterName : afterName[..endIdx]).Trim();
                    }
                    values[name] = val;
                }

                return (values.GetValueOrDefault("username", string.Empty), values.GetValueOrDefault("password", string.Empty));
            }
        }

        var pairs = body.Split('&', StringSplitOptions.RemoveEmptyEntries);
        var form = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in pairs)
        {
            var kv = pair.Split('=', 2);
            if (kv.Length == 2)
            {
                form[Uri.UnescapeDataString(kv[0].Replace('+', ' '))] = Uri.UnescapeDataString(kv[1].Replace('+', ' '));
            }
        }

        return (form.GetValueOrDefault("username", string.Empty), form.GetValueOrDefault("password", string.Empty));
    }

    private static async Task<string> ReadBodyStringAsync(HttpListenerRequest req)
    {
        using var reader = new StreamReader(req.InputStream, req.ContentEncoding ?? Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    private static async Task WriteJsonAsync(HttpListenerResponse res, HttpStatusCode status, object payload)
    {
        res.StatusCode = (int)status;
        var json = JsonSerializer.Serialize(payload, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        res.ContentLength64 = bytes.Length;
        await res.OutputStream.WriteAsync(bytes);
        res.Close();
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    private StoreData LoadOrCreateStore()
    {
        if (File.Exists(_storePath))
        {
            try
            {
                var json = File.ReadAllText(_storePath);
                var loaded = JsonSerializer.Deserialize<StoreData>(json, JsonOptions);
                if (loaded is { Users.Count: > 0, Dishes.Count: > 0 })
                {
                    return loaded;
                }
            }
            catch
            {
                // Fall back to fresh seed data if store file is corrupted.
            }
        }

        var seeded = CreateSeedData();
        _store = seeded;
        SaveStore();
        return seeded;
    }

    private void SaveStore()
    {
        var dir = Path.GetDirectoryName(_storePath);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var json = JsonSerializer.Serialize(_store, JsonOptions);
        File.WriteAllText(_storePath, json);
    }

    private static StoreData CreateSeedData() => new()
    {
        Users =
        [
            new UserRecord
            {
                Id = "u001",
                Username = "demo",
                PasswordHash = HashPassword("123456"),
                Email = "demo@uit.edu.vn",
                FirstName = "Demo",
                LastName = "UIT",
                Phone = "0900000001",
                Birthday = "2004-01-01",
                Language = "vi",
                Sex = 0
            },
            new UserRecord
            {
                Id = "u002",
                Username = "baonv",
                PasswordHash = HashPassword("123456"),
                Email = "baonv@uit.edu.vn",
                FirstName = "Bảo",
                LastName = "Nguyễn",
                Phone = "0900000002",
                Birthday = "2004-04-20",
                Language = "vi",
                Sex = 0
            }
        ],
        Dishes =
        [
            new DishItem { Id = "d001", TenMonAn = "Bún Bò Huế", Gia = 35000, DiaChi = "123 Hàn Thuyên, Thủ Đức", MoTa = "Nước dùng đậm đà hương sả ruốc Huế.", NguoiDongGop = "baonv" },
            new DishItem { Id = "d002", TenMonAn = "Phở Bò Tái Nạm", Gia = 40000, DiaChi = "45 Võ Văn Ngân, Thủ Đức", MoTa = "Phở truyền thống, thịt bò mềm, nước dùng trong.", NguoiDongGop = "baonv" },
            new DishItem { Id = "d003", TenMonAn = "Cơm Tấm Sườn Bì Chả", Gia = 40000, DiaChi = "Khu phố 6, Linh Trung, Thủ Đức", MoTa = "Sườn nướng mật ong thơm lừng kèm nước mắm kẹo.", NguoiDongGop = "demo" },
            new DishItem { Id = "d004", TenMonAn = "Hủ Tiếu Nam Vang", Gia = 35000, DiaChi = "78 Hoàng Diệu 2, Thủ Đức", MoTa = "Tôm tươi, thịt băm, trứng cút và tóp mỡ giòn.", NguoiDongGop = "baonv" },
            new DishItem { Id = "d005", TenMonAn = "Bún Chả Hà Nội", Gia = 45000, DiaChi = "22 Đặng Văn Bi, Thủ Đức", MoTa = "Chả nướng than hoa ăn cùng nem cua bể.", NguoiDongGop = "demo" },
            new DishItem { Id = "d006", TenMonAn = "Mì Quảng Gà Trứng", Gia = 38000, DiaChi = "101 Kha Vạn Cân, Thủ Đức", MoTa = "Sợi mì vàng óng, đậu phộng rang và bánh tráng.", NguoiDongGop = "baonv" },
            new DishItem { Id = "d007", TenMonAn = "Bánh Mì Thịt Nướng", Gia = 25000, DiaChi = "Cổng sau ĐH CNTT (UIT)", MoTa = "Nhanh gọn cho buổi sáng trước giờ học.", NguoiDongGop = "demo" }
        ]
    };

    private sealed class StoreData
    {
        [JsonPropertyName("users")]
        public List<UserRecord> Users { get; set; } = [];

        [JsonPropertyName("dishes")]
        public List<DishItem> Dishes { get; set; } = [];
    }

    private sealed class UserRecord
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Birthday { get; set; } = string.Empty;
        public string Language { get; set; } = "vi";
        public int Sex { get; set; }

        public UserInfo ToPublic() => new()
        {
            Id = Id,
            Username = Username,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            Phone = Phone,
            Birthday = Birthday,
            Language = Language,
            Sex = Sex
        };
    }
}
