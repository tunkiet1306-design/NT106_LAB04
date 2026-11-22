using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB04.Bai01
{
    public partial class Bai01 : Form
    {
        public Bai01()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hàm gửi yêu cầu HTTP GET tới URL và trả về toàn bộ mã nguồn HTML dưới dạng chuỗi.
        /// </summary>
        /// <param name="szURL">Đường dẫn trang web cần lấy nội dung.</param>
        /// <returns>Chuỗi chứa nội dung HTML của trang web.</returns>
        private string getHTML(string szURL)
        {
#pragma warning disable SYSLIB0014 // WebRequest và ServicePointManager lỗi thời trên .NET mới, nhưng được dùng theo đúng giáo trình Lab
            // Bật hỗ trợ giao thức bảo mật TLS 1.2 và 1.3 để kết nối ổn định với các website HTTPS hiện đại
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            // 1. Khởi tạo WebRequest tới địa chỉ URL
            WebRequest request = WebRequest.Create(szURL);

            // Thiết lập User-Agent và Timeout nếu là giao thức HTTP/HTTPS để tránh bị server chặn (403)
            if (request is HttpWebRequest httpRequest)
            {
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36";
                httpRequest.Timeout = 10000; // Tối đa 10 giây chờ phản hồi
            }

            // 2. Nhận phản hồi (WebResponse) từ máy chủ Web
            using (WebResponse response = request.GetResponse())
            {
                // 3. Lấy luồng dữ liệu (Stream) chứa nội dung do server trả về
                using (Stream? dataStream = response.GetResponseStream())
                {
                    if (dataStream == null)
                    {
                        return string.Empty;
                    }

                    // 4. Mở luồng bằng StreamReader với bảng mã UTF-8 để hiển thị đúng tiếng Việt
                    using (StreamReader reader = new StreamReader(dataStream, Encoding.UTF8))
                    {
                        // 5. Đọc toàn bộ nội dung HTML từ đầu đến cuối và trả về
                        string responseFromServer = reader.ReadToEnd();
                        return responseFromServer;
                    }
                }
            }
#pragma warning restore SYSLIB0014
        }

        /// <summary>
        /// Hàm biên dịch mã lỗi WebException sang thông báo tiếng Việt thân thiện, rõ ràng cho người dùng.
        /// </summary>
        private string GetFriendlyErrorMessage(WebException ex, string url)
        {
            // Kiểm tra trạng thái hoặc thông điệp lỗi phân giải tên miền (DNS)
            if (ex.Status == WebExceptionStatus.NameResolutionFailure ||
                ex.Message.Contains("No such host is known", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("The remote name could not be resolved", StringComparison.OrdinalIgnoreCase))
            {
                return $"[Lỗi DNS - Không tìm thấy máy chủ]\n• Tên miền không tồn tại hoặc sai chính tả: '{url}'\n• Hoặc máy tính của bạn hiện đang mất kết nối Internet.\n\n→ Hướng dẫn: Vui lòng kiểm tra lại kết nối mạng hoặc xem lại địa chỉ web.";
            }

            switch (ex.Status)
            {
                case WebExceptionStatus.ConnectFailure:
                    return $"[Lỗi kết nối máy chủ]\n• Không thể kết nối tới máy chủ tại: '{url}'\n• Máy chủ có thể đang tắt, cổng dịch vụ bị đóng hoặc máy tính bị ngắt mạng.";

                case WebExceptionStatus.Timeout:
                    return $"[Hết thời gian chờ (Timeout)]\n• Yêu cầu tới máy chủ vượt quá thời gian cho phép (10 giây).\n• Đường truyền mạng quá chậm hoặc máy chủ không phản hồi.";

                case WebExceptionStatus.ProtocolError:
                    if (ex.Response is HttpWebResponse errorResponse)
                    {
                        return $"[Lỗi phản hồi từ máy chủ - HTTP {(int)errorResponse.StatusCode}]\n• Chi tiết: {errorResponse.StatusDescription}\n• Thông điệp: {ex.Message}";
                    }
                    return $"[Lỗi giao thức HTTP]\n{ex.Message}";

                case WebExceptionStatus.TrustFailure:
                case WebExceptionStatus.SecureChannelFailure:
                    return $"[Lỗi bảo mật SSL/TLS]\n• Không thể thiết lập kênh bảo mật an toàn.\n• Chứng chỉ SSL của website có thể đã hết hạn hoặc không hợp lệ.";

                default:
                    return $"[Lỗi mạng ({ex.Status})]\n{ex.Message}";
            }
        }

        /// <summary>
        /// Sự kiện xử lý khi người dùng nhấn nút GET (hoặc nhấn phím Enter trong ô nhập URL).
        /// </summary>
        private async void btnGet_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            // 1. Kiểm tra người dùng đã nhập URL chưa
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ URL trang web!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            // 2. Tự động bổ sung tiền tố "https://" nếu người dùng chỉ gõ tên miền (ví dụ: uit.edu.vn)
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
                txtUrl.Text = url;
            }

            // 3. Kiểm tra tính hợp lệ của cú pháp URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                MessageBox.Show("Định dạng URL không hợp lệ! URL phải bắt đầu bằng http:// hoặc https://",
                    "Lỗi URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            try
            {
                // Cập nhật trạng thái giao diện trong khi đang tải dữ liệu từ server
                btnGet.Enabled = false;
                btnGet.Text = "Loading...";
                this.Cursor = Cursors.WaitCursor;
                rtbHtmlContent.Text = "Đang tải nội dung HTML từ máy chủ...";

                // Chạy hàm getHTML trên luồng nền an toàn.
                // Bắt ngoại lệ ngay bên trong delegate của Task.Run để Visual Studio Debugger không dừng ở màn hình "Exception User-Unhandled"
                var (htmlContent, webEx, otherEx) = await Task.Run(() =>
                {
                    try
                    {
                        string content = getHTML(url);
                        return (content, (WebException?)null, (Exception?)null);
                    }
                    catch (WebException ex)
                    {
                        return (string.Empty, ex, (Exception?)null);
                    }
                    catch (Exception ex)
                    {
                        return (string.Empty, (WebException?)null, ex);
                    }
                });

                // Xử lý kết quả trả về từ luồng nền
                if (webEx != null)
                {
                    string friendlyError = GetFriendlyErrorMessage(webEx, url);
                    rtbHtmlContent.Text = $"=== THÔNG BÁO LỖI KẾT NỐI MẠNG ===\r\n\r\n{friendlyError}\r\n\r\nChi tiết kỹ thuật từ hệ thống:\r\n{webEx.Message}";
                    MessageBox.Show(friendlyError, "Lỗi kết nối mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (otherEx != null)
                {
                    rtbHtmlContent.Text = $"=== ĐÃ XẢY RA LỖI ===\r\n\r\n{otherEx.Message}";
                    MessageBox.Show("Đã xảy ra lỗi không mong muốn:\n" + otherEx.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Hiển thị nội dung HTML nhận được lên RichTextBox
                    rtbHtmlContent.Text = htmlContent;
                }
            }
            catch (Exception ex)
            {
                rtbHtmlContent.Text = $"=== ĐÃ XẢY RA LỖI ===\r\n\r\n{ex.Message}";
                MessageBox.Show("Đã xảy ra lỗi không mong muốn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục lại trạng thái nút bấm và con trỏ chuột dù thành công hay gặp lỗi
                btnGet.Enabled = true;
                btnGet.Text = "GET";
                this.Cursor = Cursors.Default;
            }
        }
    }
}

