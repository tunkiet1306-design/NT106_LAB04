using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LAB04
{
    public partial class FormClient04 : Form
    {
        private TcpClient client;

        private Thread receiveThread;

        private Dictionary<string, List<string>> movieSeats =
            new Dictionary<string, List<string>>();

        public FormClient04()
        {
            InitializeComponent();

            this.Load += FormClient04_Load;
        }

        private void FormClient04_Load(
            object sender,
            EventArgs e)
        {
            flpSeat.Controls.Clear();

            cboMovie.Items.Clear();

            cboMovie.Items.Add("Avatar");
            cboMovie.Items.Add("Avengers");
            cboMovie.Items.Add("Titanic");
            cboMovie.Items.Add("Batman");
            cboMovie.SelectedIndex = 0;
            cboMovie.SelectedIndexChanged +=
                cboMovie_SelectedIndexChanged;

            lstSelectedSeat.Items.Clear();
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(20, 70);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(32, 20);
            this.lblPort.Text = "Port";

            this.txtPort.Location = new System.Drawing.Point(120, 67);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 26);
            this.txtPort.Text = "8080";
            for (char row = 'A'; row <= 'E'; row++)
            {
                for (int col = 1; col <= 5; col++)
                {
                    Button btn = new Button();

                    btn.Text =
                        row + col.ToString();

                    btn.Width = 50;
                    btn.Height = 50;

                    btn.BackColor =
                        Color.LimeGreen;

                    btn.Click += Seat_Click;

                    flpSeat.Controls.Add(btn);
                }
            }
        }

        private void cboMovie_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            RefreshSeats();
        }

        private void Seat_Click(
            object sender,
            EventArgs e)
        {
            Button btn =
                sender as Button;

            if (btn.BackColor ==
                Color.LimeGreen)
            {
                btn.BackColor =
                    Color.Yellow;

                lstSelectedSeat.Items.Add(
                    btn.Text);
            }
            else if (btn.BackColor ==
                     Color.Yellow)
            {
                btn.BackColor =
                    Color.LimeGreen;

                lstSelectedSeat.Items.Remove(
                    btn.Text);
            }
        }

        private void btnConnect_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                client =
                    new TcpClient();
                int port;

                if (!int.TryParse(txtPort.Text, out port))
                {
                    MessageBox.Show("Port không hợp lệ!");
                    return;
                }

                client.Connect(
                    txtIP.Text,
                    port);

                receiveThread =
                    new Thread(
                        ReceiveData);

                receiveThread.IsBackground =
                    true;

                receiveThread.Start();
                RefreshSeats();
                MessageBox.Show(
                    "Kết nối thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void btnDatVe_Click(
            object sender,
            EventArgs e)
        {
            if (client == null)
            {
                MessageBox.Show(
                    "Vui lòng kết nối Server trước!");

                return;
            }

            try
            {
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show(
                        "Vui lòng nhập họ tên!");
                    return;
                }

                if (cboMovie.Text == "")
                {
                    MessageBox.Show(
                        "Vui lòng chọn phim!");
                    return;
                }

                if (lstSelectedSeat.Items.Count == 0)
                {
                    MessageBox.Show(
                        "Vui lòng chọn ghế!");
                    return;
                }

                string seats = "";

                foreach (var item in lstSelectedSeat.Items)
                {
                    seats +=
                        item.ToString() + ",";
                }

                string msg =
                    "BOOK|" +
                    txtName.Text +
                    "|" +
                    cboMovie.Text +
                    "|" +
                    seats;

                byte[] data =
                    Encoding.UTF8.GetBytes(
                        msg);

                NetworkStream stream =
                    client.GetStream();

                stream.Write(
                    data,
                    0,
                    data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void ReceiveData()
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

                    if (msg.StartsWith("UPDATE|"))
                    {
                        string[] parts =
                            msg.Split('|');

                        if (parts.Length >= 3)
                        {
                            string movie =
                                parts[1];

                            string[] seats =
                                parts[2].Split(
                                    ',',
                                    StringSplitOptions.RemoveEmptyEntries);

                            if (!movieSeats.ContainsKey(movie))
                            {
                                movieSeats[movie] =
                                    new List<string>();
                            }

                            foreach (string seat in seats)
                            {
                                if (!movieSeats[movie]
                                    .Contains(seat))
                                {
                                    movieSeats[movie]
                                        .Add(seat);
                                }
                            }

                            Invoke(new Action(() =>
                            {
                                RefreshSeats();
                            }));
                        }
                    }
                    else if (msg == "FAILED")
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show(
                                "Một trong các ghế đã được đặt!");
                        }));
                    }
                    else if (msg == "SUCCESS")
                    {
                        Invoke(new Action(() =>
                        {
                            lstSelectedSeat.Items.Clear();

                            RefreshSeats();

                            MessageBox.Show(
                                "Đặt vé thành công!");
                        }));
                    }
                }
            }
            catch
            {
            }
        }

        private void RefreshSeats()
        {
            foreach (Control c in flpSeat.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = Color.LimeGreen;
                    btn.Enabled = true;
                }
            }

            lstSelectedSeat.Items.Clear();

            string movie = cboMovie.Text;

            if (movieSeats.ContainsKey(movie))
            {
                foreach (string seat in movieSeats[movie])
                {
                    UpdateSeat(seat);
                }
            }
        }

        private void UpdateSeat(
            string seat)
        {
            foreach (Control c in flpSeat.Controls)
            {
                if (c is Button btn)
                {
                    if (btn.Text == seat)
                    {
                        btn.BackColor =
                            Color.Red;

                        btn.Enabled =
                            false;
                    }
                }
            }
        }

        private void btnXoa_Click(
            object sender,
            EventArgs e)
        {
            foreach (Control c in flpSeat.Controls)
            {
                if (c is Button btn)
                {
                    if (btn.BackColor ==
                        Color.Yellow)
                    {
                        btn.BackColor =
                            Color.LimeGreen;
                    }
                }
            }

            lstSelectedSeat.Items.Clear();

            RefreshSeats();
        }

        private void btnThoat_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                }
            }
            catch
            {
            }

            Close();
        }
    }
}