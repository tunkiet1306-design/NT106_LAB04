using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net.Http;

namespace LAB04.Bai04
{
    public partial class Bai04 : Form
    {
        private List<Movie> movies = new List<Movie>();

        public Bai04()
        {
            InitializeComponent();
        }
        private void btnBook_Click(
    object sender,
    EventArgs e)
        {
            if (lstMovie.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn phim!");
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtCustomer.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên khách hàng!");
                return;
            }

            Movie movie =
                movies[lstMovie.SelectedIndex];

            int quantity =
                (int)numTicket.Value;

            decimal ticketPrice =
                90000;

            decimal total =
                quantity * ticketPrice;

            lblResult.Text =
                $"Khách hàng: {txtCustomer.Text}\n" +
                $"Phim: {movie.Title}\n" +
                $"Số vé: {quantity}\n" +
                $"Đơn giá: {ticketPrice:N0} VNĐ\n" +
                $"Thanh toán: {total:N0} VNĐ";

            MessageBox.Show(
                "Đặt vé thành công!");
        }
        private async void btnLoadMovie_Click(object sender, EventArgs e)
        {
            try
            {
                movies.Clear();
                lstMovie.Items.Clear();

                HttpClient client = new HttpClient();

                string html =
                    await client.GetStringAsync(
                        "https://betacinemas.vn/phim.htm");

                HtmlAgilityPack.HtmlDocument doc =
                    new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                File.WriteAllText(
                    Path.Combine(
                        Application.StartupPath,
                        "beta_real.html"),
                    html);

                var links =
                    doc.DocumentNode.SelectNodes("//a[@href]");

                if (links == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy dữ liệu.");
                    return;
                }

                progressBar1.Maximum = links.Count;
                progressBar1.Value = 0;

                string[] ignore =
                {
                    "Đăng nhập",
                    "Đăng ký",
                    "Phim",
                    "Rạp",
                    "Giá vé",
                    "Beta",
                    "TIN MỚI",
                    "ƯU ĐÃI",
                    "Thành viên",
                    "Nhượng quyền",
                    "LỊCH CHIẾU",
                    "MUA VÉ",
                    "Tuyển dụng",
                    "Giới thiệu",
                    "Liên hệ",
                    "F.A.Q",
                    "Hoạt động xã hội",
                    "Điều khoản",
                    "Chính sách",
                    "Hướng dẫn",
                    "Bảo mật",
                    "Hoàn vé",
                    "Online",
                    "SUẤT CHIẾU ĐẶC BIỆT"
                };

                foreach (var link in links)
                {
                    if (progressBar1.Value <
                        progressBar1.Maximum)
                    {
                        progressBar1.Value++;
                    }

                    string title =
                        HtmlEntity.DeEntitize(
                            link.InnerText).Trim();

                    string href =
                        link.GetAttributeValue(
                            "href",
                            "");

                    if (string.IsNullOrWhiteSpace(title))
                        continue;

                    if (title.Length < 5)
                        continue;

                    bool skip =
                        ignore.Any(x =>
                            title.Contains(
                                x,
                                StringComparison.OrdinalIgnoreCase));

                    if (skip)
                        continue;

                    if (!movies.Any(x =>
                        x.Title.Equals(
                            title,
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        movies.Add(
                            new Movie
                            {
                                Title = title,
                                DetailUrl = href
                            });
                    }
                }

                movies = movies
                    .OrderBy(x => x.Title)
                    .ToList();

                foreach (Movie movie in movies)
                {
                    lstMovie.Items.Add(
                        movie.Title);
                }

                string json =
                    JsonConvert.SerializeObject(
                        movies,
                        Formatting.Indented);

                string jsonPath =
                    Path.Combine(
                        Application.StartupPath,
                        "movie.json");

                File.WriteAllText(
                    jsonPath,
                    json);

                MessageBox.Show(
                    $"Đã lưu {movies.Count} phim vào movie.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void lstMovie_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (lstMovie.SelectedIndex < 0)
                return;

            Movie movie =
                movies[lstMovie.SelectedIndex];

            try
            {
                string url =
                    movie.DetailUrl;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (!url.StartsWith("http"))
                    {
                        url =
                            "https://betacinemas.vn" +
                            url;
                    }

                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                }
            }
            catch
            {
            }
        }
    }
}