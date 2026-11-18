using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LAB04.Bai07;

/// <summary>
/// Async REST API client for Bài 07 ("Hôm nay ăn gì?").
/// Communicates over HTTP with either the embedded local server or a remote compatible API.
/// </summary>
public sealed class FoodApiClient : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public FoodApiClient(string baseUrl = LocalFoodApiServer.DefaultBaseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"),
            Timeout = TimeSpan.FromSeconds(15)
        };
    }

    public string? AccessToken { get; private set; }
    public string TokenType { get; private set; } = "Bearer";
    public UserInfo? CurrentUser { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken) && CurrentUser is not null;

    public void SetBaseUrl(string baseUrl)
    {
        var normalized = baseUrl.Trim().TrimEnd('/') + "/";
        _httpClient.BaseAddress = new Uri(normalized);
        Logout();
    }

    public void Logout()
    {
        AccessToken = null;
        TokenType = "Bearer";
        CurrentUser = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<UserInfo> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        using var form = new MultipartFormDataContent
        {
            { new StringContent(username), "username" },
            { new StringContent(password), "password" }
        };

        using var response = await _httpClient.PostAsync("auth/token", form, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Đăng nhập thất bại ({(int)response.StatusCode})."));
        }

        var token = JsonSerializer.Deserialize<TokenResponse>(body, JsonOptions)
            ?? throw new InvalidOperationException("Phản hồi token không hợp lệ.");

        AccessToken = token.AccessToken;
        TokenType = string.IsNullOrWhiteSpace(token.TokenType) ? "Bearer" : token.TokenType;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenType, AccessToken);

        CurrentUser = await GetMeAsync(ct);
        return CurrentUser;
    }

    public async Task<UserInfo> GetMeAsync(CancellationToken ct = default)
    {
        using var response = await _httpClient.GetAsync("api/v1/user/me", ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Không thể lấy thông tin người dùng ({(int)response.StatusCode})."));
        }

        return JsonSerializer.Deserialize<UserInfo>(body, JsonOptions)
            ?? throw new InvalidOperationException("Dữ liệu người dùng không hợp lệ.");
    }

    public async Task<UserInfo> SignUpAsync(SignUpRequest request, CancellationToken ct = default)
    {
        using var content = SerializeJson(request);
        using var response = await _httpClient.PostAsync("api/v1/user/signup", content, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Đăng ký thất bại ({(int)response.StatusCode})."));
        }

        return JsonSerializer.Deserialize<UserInfo>(body, JsonOptions) ?? new UserInfo { Username = request.Username };
    }

    public async Task<DishPageResponse> GetDishesAsync(bool onlyMine, int current, int pageSize, CancellationToken ct = default)
    {
        var endpoint = onlyMine ? "api/v1/monan/my-dishes" : "api/v1/monan/all";
        using var content = SerializeJson(new PaginationRequest { Current = current, PageSize = pageSize });
        using var response = await _httpClient.PostAsync(endpoint, content, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Không thể tải danh sách món ăn ({(int)response.StatusCode})."));
        }

        return JsonSerializer.Deserialize<DishPageResponse>(body, JsonOptions) ?? new DishPageResponse();
    }

    public async Task<DishItem> AddDishAsync(CreateDishRequest request, CancellationToken ct = default)
    {
        using var content = SerializeJson(request);
        using var response = await _httpClient.PostAsync("api/v1/monan/add", content, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Không thể thêm món ăn ({(int)response.StatusCode})."));
        }

        return JsonSerializer.Deserialize<DishItem>(body, JsonOptions)
            ?? throw new InvalidOperationException("Phản hồi món ăn không hợp lệ.");
    }

    public async Task DeleteDishAsync(string dishId, CancellationToken ct = default)
    {
        using var response = await _httpClient.DeleteAsync($"api/v1/monan/{Uri.EscapeDataString(dishId)}", ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(ExtractErrorMessage(body, $"Không thể xóa món ăn ({(int)response.StatusCode})."));
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private static StringContent SerializeJson(object payload) =>
        new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    private static string ExtractErrorMessage(string body, string fallback)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return fallback;
        }

        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("detail", out var detail))
            {
                return detail.ValueKind == JsonValueKind.String
                    ? detail.GetString() ?? fallback
                    : detail.ToString();
            }
        }
        catch
        {
            // Return fallback if response body is not JSON.
        }

        return fallback;
    }
}
