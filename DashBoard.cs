namespace LAB04
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void OpenExercise(Form form)
        {
            Hide();
            form.FormClosed += (_, _) => Show();
            form.Show();
        }

        private void btnBai01_Click(object? sender, EventArgs e) => OpenExercise(new Bai01.Bai01());

        private void btnBai02_Click(object? sender, EventArgs e) => OpenExercise(new Bai02.Bai02());

        private void btnBai03_Click(object? sender, EventArgs e) => OpenExercise(new Bai03.Bai3());

        private void btnBai04_Click(object? sender, EventArgs e) => OpenExercise(new Bai04.Bai04());

        private void btnBai05_Click(object? sender, EventArgs e) => OpenExercise(new Bai05.Bai05());

        private void btnBai06_Click(object? sender, EventArgs e) => OpenExercise(new Bai06.Bai06());

        private void btnBai07_Click(object? sender, EventArgs e) => OpenExercise(new Bai07.Bai07());
    }
}
