using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LAB04.Bai02
{
    public partial class Bai02 : Form
    {
        private CancellationTokenSource? _downloadCts;

        public Bai02()
        {
            InitializeComponent();

#pragma warning disable SYSLIB0014
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
#pragma warning restore SYSLIB0014

            // Kiểm tra ổ đĩa mặc định: nếu máy không có ổ E:\ thì tự động chuyển về Desktop
            string defaultPath = txtFilePath.Text.Trim();
            string? rootDrive = Path.GetPathRoot(defaultPath);
            if (string.IsNullOrEmpty(rootDrive) || !Directory.Exists(rootDrive))
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                txtFilePath.Text = Path.Combine(desktopPath, "uit.html");
            }

            // Nhấp đúp chuột vào ô đường dẫn để mở hộp thoại chọn nơi lưu file
            txtFilePath.DoubleClick += TxtFilePath_DoubleClick;
        }

        private void TxtFilePath_DoubleClick(object? sender, EventArgs e)
        {
            using SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                FileName = "uit.html",
                Title = "Chọn vị trí lưu file HTML"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = saveDialog.FileName;
            }
        }

        private async void btnDownload_Click(object? sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            string fileUrl = txtFilePath.Text.Trim();

            // 1. Kiểm tra và chuẩn hóa URL
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ URL trang web.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
                txtUrl.Text = url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? validUri) ||
                (validUri.Scheme != Uri.UriSchemeHttp && validUri.Scheme != Uri.UriSchemeHttps))
            {
                MessageBox.Show("Địa chỉ URL không đúng định dạng.", "Lỗi URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Kiểm tra ổ đĩa và tự động tạo thư mục lưu file nếu chưa tồn tại
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                MessageBox.Show("Vui lòng nhập đường dẫn lưu file.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFilePath.Focus();
                return;
            }

            try
            {
                string? rootDrive = Path.GetPathRoot(fileUrl);
                if (string.IsNullOrEmpty(rootDrive) || !Directory.Exists(rootDrive))
                {
                    MessageBox.Show($"Ổ đĩa '{rootDrive}' không tồn tại trên máy.\nHãy nhấp đúp chuột vào ô đường dẫn để chọn thư mục lưu hợp lệ.",
                        "Lỗi đường dẫn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string? directory = Path.GetDirectoryName(fileUrl);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đường dẫn lưu file không hợp lệ:\n{ex.Message}", "Lỗi đường dẫn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Chuẩn bị hủy an toàn nếu người dùng đóng Form giữa chừng
            _downloadCts?.Cancel();
            _downloadCts?.Dispose();
            _downloadCts = new CancellationTokenSource();
            CancellationToken token = _downloadCts.Token;

            btnDownload.Enabled = false;
            btnDownload.Text = "Đang tải...";
            rtbHtmlContent.Clear();

            try
            {
#pragma warning disable SYSLIB0014
                using WebClient myClient = new WebClient();
#pragma warning restore SYSLIB0014

                myClient.Encoding = Encoding.UTF8;
                myClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                myClient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                using CancellationTokenRegistration registration = token.Register(() => myClient.CancelAsync());

                // Tải file về đường dẫn chỉ định
                await myClient.DownloadFileTaskAsync(validUri, fileUrl);

                // Đọc nội dung bằng OpenRead và hiển thị lên RichTextBox
                using Stream response = await myClient.OpenReadTaskAsync(validUri);
                using StreamReader reader = new StreamReader(response, Encoding.UTF8);
                string htmlContent = await reader.ReadToEndAsync();

                if (IsDisposed || !IsHandleCreated) return;

                rtbHtmlContent.Text = htmlContent;
                MessageBox.Show($"Tải trang web thành công!\nFile đã lưu tại: {fileUrl}", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException webEx) when (webEx.Status == WebExceptionStatus.RequestCanceled || token.IsCancellationRequested)
            {
                // Bỏ qua nếu người dùng chủ động đóng form khi đang tải
            }
            catch (WebException webEx)
            {
                if (!IsDisposed)
                {
                    string detail = webEx.InnerException != null ? $"\nChi tiết: {webEx.InnerException.Message}" : string.Empty;
                    MessageBox.Show($"Không thể tải dữ liệu từ Web Server:\n{webEx.Message}{detail}", "Lỗi kết nối / Ghi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    MessageBox.Show($"Đã xảy ra lỗi:\n{ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    btnDownload.Enabled = true;
                    btnDownload.Text = "Download";
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _downloadCts?.Cancel();
            _downloadCts?.Dispose();
            _downloadCts = null;
            base.OnFormClosing(e);
        }
    }
}