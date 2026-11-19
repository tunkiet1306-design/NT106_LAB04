using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace LAB04.Bai04
{
    public class BetaCinemaScraper
    {
        public const string TargetUrl = "https://betacinemas.vn/phim.htm";
        private const string BaseHost = "https://betacinemas.vn/";

        private static readonly HttpClient _http = CreateHttpClient();

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static readonly decimal[] StandardPrices = { 45000m, 70000m, 90000m, 100000m, 65000m };
        private static readonly int[][] RoomPresets =
        {
            new[] { 1, 2, 3 },
            new[] { 2, 3 },
            new[] { 1, 3 },
            new[] { 1, 2 }
        };

        public string JsonFilePath { get; }

        public BetaCinemaScraper()
        {
            JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movies.json");
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                AutomaticDecompression = DecompressionMethods.All,
                AllowAutoRedirect = true
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("vi-VN,vi;q=0.9,en-US;q=0.8,en;q=0.7");

            return client;
        }

        public async Task<ScrapeResult> ScrapeMoviesAsync(
            IProgress<CrawlProgress>? progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new ScrapeResult { JsonFilePath = JsonFilePath };
            List<Movie> movies = new List<Movie>();

            try
            {
                progress?.Report(new CrawlProgress(8, $"Đang kết nối tới {TargetUrl}..."));

                using HttpResponseMessage response = await _http.GetAsync(TargetUrl, cancellationToken);
                response.EnsureSuccessStatusCode();

                progress?.Report(new CrawlProgress(20, "Đang tải mã nguồn HTML từ Beta Cinemas..."));
                string html = await response.Content.ReadAsStringAsync(cancellationToken);

                progress?.Report(new CrawlProgress(30, "Đang phân tích cấu trúc DOM bằng HtmlAgilityPack..."));
                movies = await Task.Run(() => ParseMoviesFromHtml(html), cancellationToken);

                if (movies.Count == 0)
                {
                    throw new InvalidOperationException("Không tìm thấy dữ liệu phim trong trang HTML trả về.");
                }

                result.Message = $"Đã trích xuất thành công {movies.Count} bộ phim trực tiếp từ Beta Cinemas.";
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                result.UsedFallback = true;
                progress?.Report(new CrawlProgress(35, "Không thể lấy dữ liệu trực tuyến, đang kích hoạt chế độ dự phòng..."));

                List<Movie>? cached = LoadFromJson();
                if (cached != null && cached.Count > 0)
                {
                    movies = cached;
                    result.Message = $"Mạng/Web không khả dụng ({ex.Message}). Đã nạp {movies.Count} phim từ file movies.json đã lưu.";
                }
                else
                {
                    movies = GetFallbackSampleMovies();
                    result.Message = $"Không thể kết nối trực tiếp tới betacinemas.vn ({ex.Message}). Đã tạo dữ liệu dự phòng và lưu vào movies.json.";
                }
            }

            progress?.Report(new CrawlProgress(42, "Đang lưu danh sách phim tóm tắt vào file movies.json..."));
            await SaveToJsonAsync(movies, cancellationToken);

            int total = movies.Count;
            for (int i = 0; i < total; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Movie movie = movies[i];
                int percent = 45 + (int)Math.Round(((i + 1) / (double)total) * 53);
                progress?.Report(new CrawlProgress(percent, $"Đang tải banner ({i + 1}/{total}): {movie.Title}"));

                Image poster = await DownloadPosterSafeAsync(movie.PosterUrl, movie.Title, cancellationToken);
                result.Posters[movie.Id] = poster;
            }

            result.Movies = movies;
            progress?.Report(new CrawlProgress(100, $"Hoàn tất! Đã nạp {movies.Count} bộ phim và lưu vào movies.json."));
            return result;
        }

        private List<Movie> ParseMoviesFromHtml(string html)
        {
            // FIX LỖI CS0104: Chỉ định rõ HtmlAgilityPack.HtmlDocument để không trùng với System.Windows.Forms.HtmlDocument
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var movies = new List<Movie>();
            var seenUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            HtmlNode? scopeNode = doc.DocumentNode.SelectSingleNode("//div[@id='tab-1']") ?? doc.DocumentNode;
            var linkNodes = scopeNode.SelectNodes(".//a[contains(@href, 'chi-tiet-phim')]");

            if ((linkNodes == null || linkNodes.Count == 0) && scopeNode != doc.DocumentNode)
            {
                linkNodes = doc.DocumentNode.SelectNodes("//a[contains(@href, 'chi-tiet-phim')]");
            }

            if (linkNodes == null)
            {
                return movies;
            }

            int index = 0;
            foreach (HtmlNode link in linkNodes)
            {
                string rawHref = WebUtility.HtmlDecode(link.GetAttributeValue("href", string.Empty)).Trim();
                if (string.IsNullOrWhiteSpace(rawHref)) continue;

                string detailUrl = ResolveUrl(BaseHost, rawHref);
                if (!seenUrls.Add(detailUrl)) continue;

                HtmlNode cardNode = FindMovieCardContainer(link);

                string title = ExtractTitle(cardNode, link);
                if (string.IsNullOrWhiteSpace(title)) continue;

                string posterUrl = ExtractPosterUrl(cardNode);
                (string genre, string duration) = ExtractMetaInfo(cardNode);

                index++;
                movies.Add(new Movie
                {
                    Id = index,
                    Title = title,
                    DetailUrl = detailUrl,
                    PosterUrl = posterUrl,
                    Genre = genre,
                    Duration = duration,
                    StandardPrice = StandardPrices[(index - 1) % StandardPrices.Length],
                    Rooms = new List<int>(RoomPresets[(index - 1) % RoomPresets.Length])
                });
            }

            return movies;
        }

        private static HtmlNode FindMovieCardContainer(HtmlNode linkNode)
        {
            HtmlNode? current = linkNode.ParentNode;
            int depth = 0;

            while (current != null && depth < 6)
            {
                bool hasImage = current.SelectSingleNode(".//img") != null;
                var distinctLinks = current.SelectNodes(".//a[contains(@href, 'chi-tiet-phim')]")
                    ?.Select(n => n.GetAttributeValue("href", ""))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count() ?? 0;

                if (hasImage && distinctLinks == 1) return current;
                if (distinctLinks > 1) break;

                current = current.ParentNode;
                depth++;
            }

            return linkNode.ParentNode ?? linkNode;
        }

        private static string ExtractTitle(HtmlNode cardNode, HtmlNode fallbackLink)
        {
            HtmlNode? headingLink = cardNode.SelectSingleNode(".//h3//a") ?? cardNode.SelectSingleNode(".//h3");
            string rawTitle = headingLink?.InnerText ?? fallbackLink.InnerText;

            if (string.IsNullOrWhiteSpace(rawTitle))
                rawTitle = fallbackLink.GetAttributeValue("title", string.Empty);

            if (string.IsNullOrWhiteSpace(rawTitle))
            {
                HtmlNode? img = cardNode.SelectSingleNode(".//img");
                rawTitle = img?.GetAttributeValue("alt", string.Empty) ?? string.Empty;
            }

            return NormalizeWhitespace(WebUtility.HtmlDecode(rawTitle));
        }

        private static string ExtractPosterUrl(HtmlNode cardNode)
        {
            var images = cardNode.SelectNodes(".//img");
            if (images == null) return string.Empty;

            foreach (HtmlNode img in images)
            {
                string src = img.GetAttributeValue("src", string.Empty);
                if (string.IsNullOrWhiteSpace(src) || src.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    src = img.GetAttributeValue("data-src", string.Empty);

                if (string.IsNullOrWhiteSpace(src))
                    src = img.GetAttributeValue("data-original", string.Empty);

                if (string.IsNullOrWhiteSpace(src) ||
                    src.Contains("/icons/", StringComparison.OrdinalIgnoreCase) ||
                    src.Contains("label", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                return ResolveUrl(BaseHost, WebUtility.HtmlDecode(src).Trim());
            }

            return string.Empty;
        }

        private static (string Genre, string Duration) ExtractMetaInfo(HtmlNode cardNode)
        {
            string genre = "Đang cập nhật";
            string duration = "Đang cập nhật";

            var listItems = cardNode.SelectNodes(".//li");
            if (listItems == null) return (genre, duration);

            foreach (HtmlNode li in listItems)
            {
                string text = NormalizeWhitespace(WebUtility.HtmlDecode(li.InnerText));
                if (text.StartsWith("Thể loại:", StringComparison.OrdinalIgnoreCase))
                {
                    string val = text.Substring("Thể loại:".Length).Trim();
                    if (!string.IsNullOrWhiteSpace(val)) genre = val;
                }
                else if (text.StartsWith("Thời lượng:", StringComparison.OrdinalIgnoreCase))
                {
                    string val = text.Substring("Thời lượng:".Length).Trim();
                    if (!string.IsNullOrWhiteSpace(val)) duration = val;
                }
            }

            return (genre, duration);
        }

        private async Task<Image> DownloadPosterSafeAsync(string posterUrl, string movieTitle, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(posterUrl))
            {
                try
                {
                    byte[] bytes = await _http.GetByteArrayAsync(posterUrl, token);
                    if (bytes.Length > 0)
                    {
                        using var ms = new MemoryStream(bytes);
                        using var temp = Image.FromStream(ms);
                        return new Bitmap(temp);
                    }
                }
                catch
                {
                    // Dùng poster dự phòng bên dưới
                }
            }

            return CreateFallbackPoster(movieTitle);
        }

        public static Bitmap CreateFallbackPoster(string movieTitle)
        {
            var bmp = new Bitmap(180, 250);
            using Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var bgBrush = new LinearGradientBrush(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                Color.FromArgb(30, 41, 59),
                Color.FromArgb(15, 23, 42),
                LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(bgBrush, 0, 0, bmp.Width, bmp.Height);

            using var accentBrush = new SolidBrush(Color.FromArgb(234, 88, 12));
            g.FillRectangle(accentBrush, 0, 0, bmp.Width, 8);

            using var badgeFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using var titleFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            using var whiteBrush = new SolidBrush(Color.White);
            using var mutedBrush = new SolidBrush(Color.FromArgb(251, 146, 60));

            var centerFmt = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            g.DrawString("BETA CINEMAS", badgeFont, mutedBrush, new RectangleF(10, 28, 160, 24), centerFmt);
            g.DrawString(movieTitle, titleFont, whiteBrush, new RectangleF(12, 65, 156, 150), centerFmt);

            return bmp;
        }

        public async Task SaveToJsonAsync(List<Movie> movies, CancellationToken token = default)
        {
            string json = JsonSerializer.Serialize(movies, _jsonOptions);
            await File.WriteAllTextAsync(JsonFilePath, json, Encoding.UTF8, token);
        }

        public List<Movie>? LoadFromJson()
        {
            if (!File.Exists(JsonFilePath)) return null;

            try
            {
                string json = File.ReadAllText(JsonFilePath, Encoding.UTF8);
                return JsonSerializer.Deserialize<List<Movie>>(json, _jsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static string ResolveUrl(string baseUrl, string relativeOrAbsolute)
        {
            if (Uri.TryCreate(relativeOrAbsolute, UriKind.Absolute, out Uri? absUri))
                return absUri.AbsoluteUri;

            if (Uri.TryCreate(new Uri(baseUrl), relativeOrAbsolute, out Uri? resolved))
                return resolved.AbsoluteUri;

            return relativeOrAbsolute;
        }

        private static string NormalizeWhitespace(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return string.Join(" ", input.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private static List<Movie> GetFallbackSampleMovies()
        {
            return new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Lật Mặt 7: Một Điều Ước",
                    DetailUrl = "https://betacinemas.vn/phim.htm",
                    PosterUrl = "",
                    Genre = "Tâm lý, Gia đình",
                    Duration = "138 phút",
                    StandardPrice = 70000m,
                    Rooms = new List<int> { 1, 2, 3 }
                },
                new Movie
                {
                    Id = 2,
                    Title = "Đào, Phở và Piano",
                    DetailUrl = "https://betacinemas.vn/phim.htm",
                    PosterUrl = "",
                    Genre = "Lịch sử, Tâm lý",
                    Duration = "100 phút",
                    StandardPrice = 45000m,
                    Rooms = new List<int> { 1, 2, 3 }
                },
                new Movie
                {
                    Id = 3,
                    Title = "Mai",
                    DetailUrl = "https://betacinemas.vn/phim.htm",
                    PosterUrl = "",
                    Genre = "Tình cảm, Tâm lý",
                    Duration = "131 phút",
                    StandardPrice = 100000m,
                    Rooms = new List<int> { 2, 3 }
                },
                new Movie
                {
                    Id = 4,
                    Title = "Cái Giá Của Hạnh Phúc",
                    DetailUrl = "https://betacinemas.vn/phim.htm",
                    PosterUrl = "",
                    Genre = "Tâm lý, Gia đình",
                    Duration = "115 phút",
                    StandardPrice = 90000m,
                    Rooms = new List<int> { 1, 3 }
                }
            };
        }
    }
}