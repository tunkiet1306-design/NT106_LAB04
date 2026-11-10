using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace LAB04.Bai03
{
    public partial class Bai3 : Form
    {
        private static readonly HttpClient HttpClient = CreateHttpClient();
        private bool _isWebViewInitialized;

        public Bai3()
        {
            InitializeComponent();
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
                AllowAutoRedirect = true
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            return client;
        }

        private async void Bai3_Load(object? sender, EventArgs e)
        {
            await EnsureWebViewReadyAsync();
        }

        private async Task<bool> EnsureWebViewReadyAsync()
        {
            if (_isWebViewInitialized && webView.CoreWebView2 != null)
            {
                return true;
            }

            try
            {
                lblStatus.Text = "Đang khởi tạo WebView2...";
                await webView.EnsureCoreWebView2Async();
                var core = webView.CoreWebView2 ?? throw new InvalidOperationException("CoreWebView2 chưa được khởi tạo.");

                core.NavigationStarting += (_, args) =>
                {
                    if (Uri.TryCreate(args.Uri, UriKind.Absolute, out var navUri) &&
                        (navUri.Scheme == Uri.UriSchemeHttp || navUri.Scheme == Uri.UriSchemeHttps))
                    {
                        txtUrl.Text = navUri.ToString();
                    }
                    lblStatus.Text = $"Đang tải {args.Uri}...";
                };

                core.SourceChanged += (_, _) =>
                {
                    if (webView.Source != null &&
                        (webView.Source.Scheme == Uri.UriSchemeHttp || webView.Source.Scheme == Uri.UriSchemeHttps))
                    {
                        txtUrl.Text = webView.Source.ToString();
                    }
                };

                core.NavigationCompleted += (_, args) =>
                {
                    lblStatus.Text = args.IsSuccess
                        ? $"Đã tải xong: {webView.Source}"
                        : $"Không thể tải trang (lỗi: {args.WebErrorStatus})";
                };

                _isWebViewInitialized = true;
                lblStatus.Text = "Sẵn sàng";
                return true;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Không thể khởi tạo WebView2";
                MessageBox.Show(
                    this,
                    $"Không thể khởi tạo WebView2. Hãy đảm bảo máy đã cài đặt Microsoft Edge WebView2 Runtime.\n\nChi tiết: {ex.Message}",
                    "Lỗi WebView2",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private async void btnLoad_Click(object? sender, EventArgs e)
        {
            await NavigateToInputUrlAsync();
        }

        private async void txtUrl_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await NavigateToInputUrlAsync();
            }
        }

        private async Task NavigateToInputUrlAsync()
        {
            if (!TryGetTargetUri(out var uri))
            {
                MessageBox.Show(this, "Vui lòng nhập URL hợp lệ (vd: https://uit.edu.vn/).", "URL không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            txtUrl.Text = uri.ToString();

            if (!await EnsureWebViewReadyAsync())
            {
                return;
            }

            webView.CoreWebView2.Navigate(uri.ToString());
        }

        private async void btnReload_Click(object? sender, EventArgs e)
        {
            if (!await EnsureWebViewReadyAsync())
            {
                return;
            }

            if (webView.Source != null &&
                (webView.Source.Scheme == Uri.UriSchemeHttp || webView.Source.Scheme == Uri.UriSchemeHttps))
            {
                webView.Reload();
            }
            else
            {
                await NavigateToInputUrlAsync();
            }
        }

        private async void btnViewSource_Click(object? sender, EventArgs e)
        {
            if (!TryGetTargetUri(out var uri))
            {
                MessageBox.Show(this, "Vui lòng nhập hoặc tải một URL hợp lệ trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SetBusy(true, $"Đang lấy mã nguồn từ {uri}...");
                string html = await FetchHtmlAsync(uri);
                lblStatus.Text = $"Đã lấy mã nguồn: {uri}";

                ShowViewSourceForm(uri.ToString(), html);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi lấy mã nguồn";
                MessageBox.Show(this, $"Không thể lấy mã nguồn HTML:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnDownFiles_Click(object? sender, EventArgs e)
        {
            if (!TryGetTargetUri(out var uri))
            {
                MessageBox.Show(this, "Vui lòng nhập hoặc tải một URL hợp lệ trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Title = "Lưu file HTML",
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                DefaultExt = "html",
                FileName = SanitizeFileName($"{uri.Host}.html", "page.html")
            };

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                SetBusy(true, $"Đang tải HTML từ {uri}...");
                string html = await FetchHtmlAsync(uri);
                await File.WriteAllTextAsync(saveDialog.FileName, html, Encoding.UTF8);

                lblStatus.Text = $"Đã lưu HTML: {saveDialog.FileName}";
                MessageBox.Show(this, $"Đã tải và lưu file HTML thành công tại:\n{saveDialog.FileName}", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi tải file HTML";
                MessageBox.Show(this, $"Không thể tải file HTML:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnDownResources_Click(object? sender, EventArgs e)
        {
            if (!TryGetTargetUri(out var uri))
            {
                MessageBox.Show(this, "Vui lòng nhập hoặc tải một URL hợp lệ trước.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var folderDialog = new FolderBrowserDialog
            {
                Description = "Chọn thư mục lưu các hình ảnh của website",
                UseDescriptionForTitle = true
            };

            if (folderDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                SetBusy(true, $"Đang phân tích hình ảnh từ {uri}...");
                string html = await FetchHtmlAsync(uri);

                var imageUrls = ExtractImageUrls(html, uri);
                if (imageUrls.Count == 0)
                {
                    lblStatus.Text = "Không tìm thấy hình ảnh nào trên trang";
                    MessageBox.Show(this, "Không tìm thấy thẻ <img> có liên kết hình ảnh hợp lệ trên website.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int downloaded = 0;
                int failed = 0;
                var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < imageUrls.Count; i++)
                {
                    Uri imgUri = imageUrls[i];
                    lblStatus.Text = $"Đang tải hình {i + 1}/{imageUrls.Count}: {imgUri}";

                    try
                    {
                        using HttpResponseMessage response = await HttpClient.GetAsync(imgUri);
                        if (!response.IsSuccessStatusCode)
                        {
                            failed++;
                            continue;
                        }

                        byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                        if (bytes.Length == 0)
                        {
                            failed++;
                            continue;
                        }

                        string? contentType = response.Content.Headers.ContentType?.MediaType;
                        string filePath = BuildUniqueFilePath(folderDialog.SelectedPath, imgUri, contentType, i + 1, usedNames);
                        await File.WriteAllBytesAsync(filePath, bytes);
                        downloaded++;
                    }
                    catch
                    {
                        failed++;
                    }
                }

                lblStatus.Text = $"Hoàn tất: đã tải {downloaded}/{imageUrls.Count} hình ảnh";
                MessageBox.Show(
                    this,
                    $"Hoàn tất tải resource!\n" +
                    $"- Tổng số link ảnh tìm thấy: {imageUrls.Count}\n" +
                    $"- Tải thành công: {downloaded}\n" +
                    $"- Thất bại: {failed}\n" +
                    $"- Thư mục lưu: {folderDialog.SelectedPath}",
                    "Kết quả Download Resources",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi tải resources";
                MessageBox.Show(this, $"Không thể tải resources:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private bool TryGetTargetUri(out Uri uri)
        {
            string raw = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(raw) && webView.Source != null)
            {
                raw = webView.Source.ToString();
            }

            if (!string.IsNullOrEmpty(raw) &&
                !raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                raw = "https://" + raw;
            }

            if (Uri.TryCreate(raw, UriKind.Absolute, out Uri? parsed) &&
                (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps))
            {
                uri = parsed;
                return true;
            }

            uri = null!;
            return false;
        }

        private static async Task<string> FetchHtmlAsync(Uri uri)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static List<Uri> ExtractImageUrls(string html, Uri baseUri)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var result = new List<Uri>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var imgNodes = doc.DocumentNode.SelectNodes("//img");
            if (imgNodes == null)
            {
                return result;
            }

            foreach (var node in imgNodes)
            {
                string? rawSrc = node.GetAttributeValue("src", string.Empty);
                if (string.IsNullOrWhiteSpace(rawSrc) || rawSrc.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    rawSrc = node.GetAttributeValue("data-src", string.Empty);
                }
                if (string.IsNullOrWhiteSpace(rawSrc) || rawSrc.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    rawSrc = node.GetAttributeValue("data-lazy-src", string.Empty);
                }

                if (string.IsNullOrWhiteSpace(rawSrc))
                {
                    continue;
                }

                rawSrc = WebUtility.HtmlDecode(rawSrc.Trim());
                if (Uri.TryCreate(baseUri, rawSrc, out Uri? resolved) &&
                    (resolved.Scheme == Uri.UriSchemeHttp || resolved.Scheme == Uri.UriSchemeHttps) &&
                    seen.Add(resolved.ToString()))
                {
                    result.Add(resolved);
                }
            }

            return result;
        }

        private static string BuildUniqueFilePath(
            string folder,
            Uri imgUri,
            string? contentType,
            int index,
            HashSet<string> usedNames)
        {
            string rawName = Path.GetFileName(imgUri.LocalPath);
            string baseName = SanitizeFileName(Path.GetFileNameWithoutExtension(rawName), $"image_{index}");
            string ext = Path.GetExtension(rawName);

            if (string.IsNullOrWhiteSpace(ext) || ext.Length > 10)
            {
                ext = InferExtensionFromContentType(contentType);
            }

            string candidate = $"{baseName}{ext}";
            int suffix = 1;
            while (!usedNames.Add(candidate) || File.Exists(Path.Combine(folder, candidate)))
            {
                candidate = $"{baseName}_{suffix}{ext}";
                suffix++;
            }

            return Path.Combine(folder, candidate);
        }

        private static string InferExtensionFromContentType(string? contentType)
        {
            return contentType?.ToLowerInvariant() switch
            {
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/webp" => ".webp",
                "image/svg+xml" => ".svg",
                "image/bmp" => ".bmp",
                "image/x-icon" or "image/vnd.microsoft.icon" => ".ico",
                _ => ".jpg"
            };
        }

        private static string SanitizeFileName(string name, string fallback)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return fallback;
            }

            char[] invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (char c in name)
            {
                sb.Append(Array.IndexOf(invalid, c) >= 0 ? '_' : c);
            }

            string sanitized = sb.ToString().Trim();
            return string.IsNullOrEmpty(sanitized) ? fallback : sanitized;
        }

        private void SetBusy(bool busy, string? statusText = null)
        {
            btnLoad.Enabled = !busy;
            btnReload.Enabled = !busy;
            btnViewSource.Enabled = !busy;
            btnDownFiles.Enabled = !busy;
            btnDownResources.Enabled = !busy;
            UseWaitCursor = busy;

            if (!string.IsNullOrEmpty(statusText))
            {
                lblStatus.Text = statusText;
            }
        }

        private void ShowViewSourceForm(string url, string htmlSource)
        {
            var sourceForm = new Form
            {
                Text = $"View Source - {url}",
                Size = new Size(900, 620),
                MinimumSize = new Size(500, 350),
                StartPosition = FormStartPosition.CenterParent
            };

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(8, 6, 8, 6),
                BackColor = Color.FromArgb(245, 247, 250)
            };

            var lblUrl = new Label
            {
                Text = $"URL: {url}",
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F)
            };

            var txtSource = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                MaxLength = 0,
                Font = new Font("Consolas", 10F),
                BackColor = Color.White,
                Text = htmlSource.Replace("\r\n", "\n").Replace("\n", Environment.NewLine)
            };

            var btnCopy = new Button
            {
                Text = "Copy HTML",
                Dock = DockStyle.Right,
                Width = 100,
                Cursor = Cursors.Hand
            };
            btnCopy.Click += (_, _) =>
            {
                if (!string.IsNullOrEmpty(txtSource.Text))
                {
                    Clipboard.SetText(txtSource.Text);
                    MessageBox.Show(sourceForm, "Đã sao chép mã nguồn HTML vào clipboard.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            topPanel.Controls.Add(lblUrl);
            topPanel.Controls.Add(btnCopy);

            sourceForm.Controls.Add(txtSource);
            sourceForm.Controls.Add(topPanel);
            sourceForm.Show(this);
        }
    }
}
