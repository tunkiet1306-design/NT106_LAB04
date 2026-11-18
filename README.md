# NT106 - LAB 04: Truyền thông với Web Server trong C#

Ứng dụng Windows Forms (.NET 10) tổng hợp 7 bài thực hành Lab 04, điều hướng từ `DashBoard`.

## 1. Cấu trúc thư mục dự án

```text
NT106_LAB04/
├── DashBoard.cs              # Form điều hướng mở Bai01 -> Bai07
├── Program.cs                # Điểm khởi chạy ứng dụng
├── LAB04.csproj              # Cấu hình project (.NET 10 WinForms)
├── Bai01/ ... Bai06/         # Thư mục mã nguồn từng bài 01 - 06
└── Bai07/                    # Bài 07: Hôm nay ăn gì? (kèm Local REST API)
```

## 2. Phân chia nhiệm vụ & Quy ước đặt tên Branch

Mỗi thành viên phụ trách thư mục bài tương ứng trên một nhánh riêng:

| Bài | Nội dung chính | Thư mục | Tên branch |
| :--- | :--- | :--- | :--- |
| **Bài 01** | Lấy & hiển thị HTML bằng `WebRequest` | `Bai01/` | `feat/bai01` |
| **Bài 02** | Download trang web & lưu file HTML bằng `WebClient` | `Bai02/` | `feat/bai02` |
| **Bài 03** | Trình duyệt Web cơ bản (`WebView2` + `HtmlAgilityPack`) | `Bai03/` | `feat/bai03` |
| **Bài 04** | Quản lý phòng vé (crawl dữ liệu phim + JSON) | `Bai04/` | `feat/bai04` |
| **Bài 05** | HTTP POST đăng nhập lấy JWT Token | `Bai05/` | `feat/bai05` |
| **Bài 06** | HTTP GET lấy thông tin user với JWT | `Bai06/` | `feat/bai06` |
| **Bài 07** | Hôm nay ăn gì? v4 (REST API CRUD + Auth + Random) | `Bai07/` | `feat/bai07` |

## 3. Quy trình làm việc với Git & Pull Request

> Nhánh mặc định của repository là `master`. **Không push trực tiếp lên `master`.**

1. **Cập nhật nhánh chính mới nhất:**
   ```powershell
   git checkout master
   git pull origin master
   ```
2. **Tạo branch cho bài của mình:**
   ```powershell
   git checkout -b feat/bai0X
   ```
3. **Code và commit theo chuẩn Conventional Commits (`feat:`, `fix:`, `docs:`):**
   ```powershell
   git add Bai0X/
   git commit -m "feat(bai0X): hoàn thiện chức năng bài 0X"
   ```
4. **Push branch lên GitHub:**
   ```powershell
   git push -u origin feat/bai0X
   ```
5. **Tạo Pull Request vào `master`:**
   - Tạo trên GitHub hoặc dùng GitHub CLI:
     ```powershell
     gh pr create --base master --head feat/bai0X --title "feat(bai0X): ..." --body "Mô tả thay đổi..."
     ```

## 4. Lưu ý quan trọng cho nhóm

- **Không chỉnh sửa chéo** thư mục bài của thành viên khác (`Bai01/` – `Bai07/`).
- Không sửa `DashBoard.cs` / `LAB04.csproj` nếu không thật sự cần thiết để tránh xung đột (conflict) khi merge.
- Không commit file đề PDF, thư mục `bin/`, `obj/`, hay file tạm lên Git.

## 5. Chạy thử dự án

```powershell
dotnet build
dotnet run
```

- **Lưu ý riêng cho Bài 07:** Ứng dụng tự động khởi chạy Local REST API tại `http://localhost:5077/` (lưu dữ liệu tại `%LocalAppData%\LAB04_Bai07\store.json`, có sẵn dữ liệu mẫu và tài khoản demo `demo` / `123456`), hoặc có thể nhập URL server khác (ví dụ `https://nt106.uitiot.vn`) trực tiếp trên giao diện.
