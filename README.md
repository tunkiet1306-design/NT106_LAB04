# LAB04 - Working with Web Server in C#

## Quy định chung

* **Quy trình làm việc trên Git:**
  * Mỗi thành viên tạo một nhánh riêng cho bài của mình từ nhánh chính `master` (ví dụ: `feat/bai01`, `feat/bai02`, ..., `feat/bai07`), không push trực tiếp lên nhánh chính.
  * Trước khi bắt đầu viết code, bắt buộc kéo code mới nhất về bằng lệnh:
    ```powershell
    git pull origin master
    ```
  * Sau khi hoàn thành và kiểm tra không còn lỗi biên dịch, push nhánh lên repo và tạo Pull Request để nhóm trưởng review và merge.

* **Quy ước thư mục và cấu trúc Form:**
  * Không chỉnh sửa file `DashBoard.cs` và `Program.cs` (trừ khi chỉ nối sự kiện mở Form bài của mình theo đúng quy ước chung).
  * Mỗi thành viên làm bài trong thư mục tương ứng tại thư mục gốc của project (`Bai01`, `Bai02`, ..., `Bai07`).
  * Toàn bộ mã nguồn và giao diện của bài nào phải nằm gọn trong 1 Form duy nhất bên trong thư mục bài đó (`BaiX.cs`, `BaiX.Designer.cs`, `BaiX.resx`), không tạo thêm file hay project ngoài.
  * **Lưu ý quan trọng về Namespace:** Tất cả các file `.cs` trong thư mục bài đều phải đặt `namespace LAB04` để liên kết thông với `DashBoard.cs`.
