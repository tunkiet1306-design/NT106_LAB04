namespace LAB04.Bai07;

public partial class DishResultForm : Form
{
    public DishResultForm()
    {
        InitializeComponent();
    }

    public DishResultForm(DishItem dish, Image? thumbnail) : this()
    {
        Text = $"Ăn {dish.TenMonAn} đi!!!";
        lblName.Text = dish.TenMonAn;
        lblPrice.Text = $"Giá: {dish.Gia:N0} VNĐ";
        lblAddress.Text = $"Địa chỉ: {(string.IsNullOrWhiteSpace(dish.DiaChi) ? "Đang cập nhật" : dish.DiaChi)}";
        lblContributor.Text = $"Đóng góp: {dish.NguoiDongGop}";
        lblDescription.Text = string.IsNullOrWhiteSpace(dish.MoTa) ? string.Empty : $"Mô tả: {dish.MoTa}";
        picDish.Image = thumbnail;
    }
}
