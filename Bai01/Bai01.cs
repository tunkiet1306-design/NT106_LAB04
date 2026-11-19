using System.Net;
using System.Text;

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
        /// 

        private string getHTML(string szURL)
        {
            // Bật hỗ trợ giao thức bảo mật TLS 1.2 để kết nối ổn định với các website HTTPS
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            // 1. Khởi tạo WebRequest tới địa chỉ URL
            WebRequest request = WebRequest.Create(szURL);

            // Thiết lập User-Agent và Timeout nếu là giao thức HTTP/HTTPS để tránh bị server chặn (403)
            if (request is HttpWebRequest httpRequest)
            {
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
                httpRequest.Timeout = 15000; // Tối đa 15 giây chờ phản hồi
            }

            // 2. Nhận phản hồi (WebResponse) từ máy chủ Web
            using (WebResponse response = request.GetResponse())
            {
                // 3. Lấy luồng dữ liệu (Stream) chứa nội dung do server trả về
                using (Stream dataStream = response.GetResponseStream())
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
        }

        /// <summary>
        /// Sự kiện xử lý khi người dùng nhấn nút GET (hoặc nhấn phím Enter trong ô nhập URL).
        /// </summary>
        /// 
        private async void btnGet_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            // Kiểm tra người dùng đã nhập URL chưa
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ URL trang web!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUrl.Focus();
                return;
            }

            // Tự động bổ sung tiền tố "http://" nếu người dùng chỉ gõ tên miền (ví dụ: uit.edu.vn)
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
                txtUrl.Text = url;
            }

            // Kiểm tra tính hợp lệ cơ bản của cú pháp URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                MessageBox.Show("Định dạng URL không hợp lệ. Ví dụ đúng: http://uit.edu.vn",
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

                // Chạy hàm getHTML trên luồng nền để giao diện Form không bị treo (Not Responding)
                string htmlContent = await Task.Run(() => getHTML(url));

                // Hiển thị nội dung HTML nhận được lên RichTextBox
                rtbHtmlContent.Text = htmlContent;
            }
            catch (WebException ex)
            {
                string errorDetail = ex.Message;
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    errorDetail = $"HTTP {(int)errorResponse.StatusCode} ({errorResponse.StatusDescription}) - {ex.Message}";
                }

                rtbHtmlContent.Clear();
                MessageBox.Show("Không thể kết nối tới Web Server:\n" + errorDetail,
                    "Lỗi kết nối mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                rtbHtmlContent.Clear();
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

