using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace LAB04
{
    public partial class FormServer04 : Form
    {
        public class Movie
        {
            public string Name { get; set; }
            public int Total { get; set; }
            public int Sold { get; set; }
            public int Revenue { get; set; }
        }

        private TcpListener listener;
        private TextBox txtPort;
        private Label lblPort;
        private List<TcpClient> clients =
            new List<TcpClient>();

        private List<string> bookedSeats =
            new List<string>();

        private List<Movie> movies =
            new List<Movie>();

        public FormServer04()
        {
            InitializeComponent();
        }

        private void FormServer04_Load(
            object sender,
            EventArgs e)
        {
            dgvMovie.Columns.Add("Movie", "Tên Phim");
            dgvMovie.Columns.Add("Total", "Tổng Vé");
            dgvMovie.Columns.Add("Sold", "Đã Bán");
            dgvMovie.Columns.Add("Remain", "Còn Lại");
            dgvMovie.Columns.Add("Revenue", "Doanh Thu");
            lblPort = new Label();
            lblPort.Text = "Port:";
            lblPort.Location = new Point(360, 145);
            lblPort.AutoSize = true;

            txtPort = new TextBox();
            txtPort.Name = "txtPort";
            txtPort.Text = "8080";
            txtPort.Location = new Point(400, 142);
            txtPort.Width = 100;

            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            bookedSeats.Clear();

            string moviePath =
                Path.Combine(
                    Application.StartupPath,
                    "movie.txt");

            string seatPath =
                Path.Combine(
                    Application.StartupPath,
                    "bookedSeats.txt");

            if (File.Exists(seatPath))
            {
                File.Delete(seatPath);
            }

            if (!File.Exists(moviePath))
            {
                File.WriteAllLines(
                    moviePath,
                    new string[]
                    {
            "Avatar,500,420,42000000",
            "Avengers,600,580,69600000",
            "Titanic,400,250,22500000",
            "Batman,450,300,28500000"
                    });
            }

            rtbLog.AppendText(
                "Đã reset toàn bộ dữ liệu\n");
        }


        private void btnReadFile_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                dgvMovie.Rows.Clear();

                movies.Clear();

                string moviePath =
                    Path.Combine(
                        Application.StartupPath,
                        "movie.txt");

                string[] lines =
                    File.ReadAllLines(moviePath);

                foreach (string line in lines)
                {
                    string[] data =
                        line.Split(',');

                    Movie m =
                        new Movie();

                    m.Name =
                        data[0];

                    m.Total =
                        int.Parse(data[1]);

                    m.Sold =
                        int.Parse(data[2]);

                    m.Revenue =
                        int.Parse(data[3]);

                    movies.Add(m);

                    dgvMovie.Rows.Add(
                        m.Name,
                        m.Total,
                        m.Sold,
                        m.Total - m.Sold,
                        m.Revenue);
                }

                rtbLog.AppendText(
                    "Đọc file thành công!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void btnStartServer_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (listener != null)
                {
                    MessageBox.Show(
                        "Server đã chạy!");
                    return;
                }

                int port;

                if (!int.TryParse(txtPort.Text, out port))
                {
                    MessageBox.Show("Port không hợp lệ!");
                    return;
                }

                listener =
                    new TcpListener(
                        IPAddress.Any,
                        port);

                listener.Start();

                Thread t =
                    new Thread(
                        ListenClient);

                t.IsBackground =
                    true;

                t.Start();

                progressBar1.Value =
                    100;

                btnStartServer.Enabled =
                    false;

                rtbLog.AppendText(
                    "Server Started...\n");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(
                    $"Socket Error: {ex.SocketErrorCode}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ListenClient()
        {
            while (true)
            {
                try
                {
                    TcpClient client =
                        listener.AcceptTcpClient();

                    clients.Add(client);

                    // Gửi danh sách ghế đã bán cho client mới

                    var groupedSeats = bookedSeats
                        .GroupBy(x => x.Split('_')[0]);

                    foreach (var group in groupedSeats)
                    {
                        string movie = group.Key;

                        string seats = string.Join(",",
                            group.Select(x => x.Split('_')[1]));

                        string msg =
                            $"UPDATE|{movie}|{seats}";

                        byte[] data =
                            Encoding.UTF8.GetBytes(msg);

                        client.GetStream().Write(
                            data,
                            0,
                            data.Length);
                    }

                    Invoke(new Action(() =>
                    {
                        rtbLog.AppendText(
                            "Client Connected\n");

                        rtbLog.ScrollToCaret();
                    }));

                    Thread t =
                        new Thread(
                            () => HandleClient(client));

                    t.IsBackground = true;

                    t.Start();
                }
                catch
                {
                    break;
                }
            }
        }

        private void HandleClient(
            TcpClient client)
        {
            try
            {
                NetworkStream stream =
                    client.GetStream();

                byte[] buffer =
                    new byte[4096];

                while (true)
                {
                    int bytesRead =
                        stream.Read(
                            buffer,
                            0,
                            buffer.Length);

                    if (bytesRead == 0)
                        break;

                    string msg =
                        Encoding.UTF8.GetString(
                            buffer,
                            0,
                            bytesRead);

                    Invoke(new Action(() =>
                    {
                        rtbLog.AppendText(
                            "Received: "
                            + msg + "\n");
                    }));

                    if (msg.StartsWith("BOOK|"))
                    {
                        string[] parts =
                            msg.Split('|');

                        string customerName =
                            parts[1];

                        string movieName =
                            parts[2];

                        string[] seats =
                            parts[3].Split(
                                ',',
                                StringSplitOptions.RemoveEmptyEntries);

                        lock (bookedSeats)
                        {
                            bool valid =
                                true;

                            foreach (string seat in seats)
                            {
                                string key =
                                    movieName + "_" + seat;

                                if (bookedSeats.Contains(key))
                                {
                                    valid = false;
                                    break;
                                }
                            }


                            if (valid)
                            {
                                foreach (string seat in seats)
                                {
                                    bookedSeats.Add(
                                        movieName + "_" + seat);
                                }

                                SaveSeats();

                                string allSeats =
                                    string.Join(",", seats);

                                Broadcast(
                                    "UPDATE|" +
                                    movieName +
                                    "|" +
                                    allSeats);

                                Movie movie =
                                  movies.Find(
                                      x => x.Name.Trim()
                                      .Equals(
                                          movieName.Trim(),
                                          StringComparison.OrdinalIgnoreCase));

                                if (movie != null)
                                {
                                    movie.Sold += seats.Length;

                                    SaveMoviesToFile();

                                    Invoke(new Action(() =>
                                    {
                                        ReloadGrid();

                                        rtbLog.AppendText(
                                            $"Updated {movie.Name} -> Sold: {movie.Sold}\n");
                                    }));
                                }
                                else
                                {
                                    Invoke(new Action(() =>
                                    {
                                        rtbLog.AppendText(
                                            $"Movie NOT FOUND: {movieName}\n");
                                    }));
                                }

                                byte[] ok =
                                    Encoding.UTF8.GetBytes(
                                        "SUCCESS");

                                stream.Write(
                                    ok,
                                    0,
                                    ok.Length);
                            }
                            else
                            {
                                byte[] fail =
                                    Encoding.UTF8.GetBytes(
                                        "FAILED");

                                stream.Write(
                                    fail,
                                    0,
                                    fail.Length);
                            }
                        }
                    }
                }

                client.Close();
            }
            catch
            {
            }
        }

        private void Broadcast(
            string message)
        {
            byte[] data =
                Encoding.UTF8.GetBytes(
                    message);

            foreach (TcpClient c in clients)
            {
                try
                {
                    NetworkStream stream =
                        c.GetStream();

                    stream.Write(
                        data,
                        0,
                        data.Length);
                }
                catch
                {
                }
            }
        }

        private void ReloadGrid()
        {
            dgvMovie.Rows.Clear();

            foreach (Movie m in movies)
            {
                dgvMovie.Rows.Add(
                    m.Name,
                    m.Total,
                    m.Sold,
                    m.Total - m.Sold,
                    m.Revenue);
            }
        }

        private void SaveMoviesToFile()
        {
            List<string> lines =
                new List<string>();

            foreach (Movie m in movies)
            {
                lines.Add(
                    $"{m.Name},{m.Total},{m.Sold},{m.Revenue}");
            }

            string moviePath =
                Path.Combine(
                    Application.StartupPath,
                    "movie.txt");

            File.WriteAllLines(
                moviePath,
                lines);
        }

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            if (listener != null)
            {
                listener.Stop();
            }

            base.OnFormClosing(e);
        }
        private void SaveSeats()
        {
            string seatPath =
                Path.Combine(
                    Application.StartupPath,
                    "bookedSeats.txt");

            File.WriteAllLines(
                seatPath,
                bookedSeats);
        }
    }
}