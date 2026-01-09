# 🛒 MyShopDaoHD - Hệ thống Quản lý Bán hàng

**MyShopDaoHD** là một giải pháp web full-stack được xây dựng trên nền tảng **.NET 8**, được thiết kế để minh họa **Kiến trúc N-Layer (N-Tầng)** chuẩn mực. Hệ thống bao gồm một RESTful Web API (hỗ trợ OData) và một Client sử dụng Razor Pages.

Dự án này đóng vai trò là tài liệu tham khảo cho việc xây dựng các ứng dụng có khả năng mở rộng cao với **Entity Framework Core 9**, **Xác thực JWT**, và khả năng truy vấn **OData**.

[Hình ảnh sơ đồ kiến trúc N-Layer]

---

## 🏗 Kiến trúc Dự án

Giải pháp được chia thành 5 project riêng biệt để đảm bảo sự phân tách nhiệm vụ (Separation of Concerns):

| Project | Tầng (Layer) | Mô tả |
| :--- | :--- | :--- |
| **MyFE** | Presentation | **Razor Pages** client. Gọi API thông qua `HttpClient` và xử lý giao diện người dùng. |
| **MyAPI** | Presentation | **ASP.NET Core Web API**. Cung cấp các REST endpoint, OData và tài liệu Swagger. |
| **Services** | Business Logic | Chứa logic nghiệp vụ, ánh xạ DTO (AutoMapper) và tạo JWT. |
| **Repositories** | Data Access | Triển khai mẫu Repository Pattern để trừu tượng hóa các thao tác database. |
| **DataAccessObjects** | Data Access | Chứa `ShopDbContext` và các cấu hình EF Core. |
| **BussinessObjects** | Core | Chứa các Entity (`Product`, `Order`, `User`...) và các DTO. |

---

## 🛠 Công nghệ sử dụng

* **Framework:** .NET 8.0
* **Database:** SQL Server
* **ORM:** Entity Framework Core 9.0.7
* **Tiêu chuẩn API:** REST & OData (Open Data Protocol)
* **Xác thực (Authentication):**
    * **API:** JWT (JSON Web Token) Bearer
    * **Frontend:** Cookie Authentication
* **Mapping:** AutoMapper 14.0
* **Tài liệu hóa:** Swagger / OpenAPI

---

## 💾 Thiết kế Database

Hệ thống được xây dựng xoay quanh các thực thể cốt lõi sau:
* **Users:** Tài khoản người dùng để xác thực.
* **Categories:** Phân loại sản phẩm.
* **Products:** Sản phẩm được bán.
* **Orders & OrderDetails:** Dữ liệu giao dịch liên kết giữa người dùng và sản phẩm.

[Hình ảnh sơ đồ Database - ERD]

---

## ⚙️ Cài đặt & Hướng dẫn chạy

### 1. Yêu cầu tiên quyết
* [Visual Studio 2022](https://visualstudio.microsoft.com/)
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server

### 2. Cấu hình Database
Mở file `appsettings.json` trong project **MyAPI**. Cập nhật chuỗi kết nối phù hợp với SQL Server của bạn:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(local);Database=ShopDB;User Id=sa;Password=your_password;Encrypt=False;TrustServerCertificate=True;"
}
```

### 3. Cập nhật Database (Migrations)
Mở **Package Manager Console** trong Visual Studio:
1.  Tại ô **Default project**, chọn `DataAccessObjects`.
2.  Chạy lệnh:

```powershell
Update-Database
```
*Hoặc sử dụng terminal tại thư mục `DataAccessObjects`:*
```bash
dotnet ef database update
```

### 4. Chạy ứng dụng
Giải pháp yêu cầu chạy song song cả API và Frontend.

1.  Chuột phải vào **Solution** trong Solution Explorer > Chọn **Set Startup Projects**.
2.  Chọn **Multiple startup projects**.
3.  Đặt Action là **Start** cho cả **MyAPI** và **MyFE**.
4.  Nhấn **F5** để chạy.

* **API URL:** `https://localhost:7017` (Swagger: `/swagger`)
* **Frontend URL:** `https://localhost:7204`

---

## 🚀 Hướng dẫn sử dụng OData

API hỗ trợ **OData**, cho phép truy vấn dữ liệu linh hoạt trên endpoint **Products** mà không cần sửa code backend.

**Endpoint:** `GET /odata/Products`

### Các ví dụ truy vấn

| Chức năng | URL Query mẫu |
| :--- | :--- |
| **Chọn cột cụ thể** | `/odata/Products?$select=Name,Price` |
| **Lọc theo giá** | `/odata/Products?$filter=Price lt 100` |
| **Tìm theo tên** | `/odata/Products?$filter=contains(Name, 'Laptop')` |
| **Sắp xếp giá (Giảm dần)** | `/odata/Products?$orderby=Price desc` |
| **Mở rộng (Join) Category** | `/odata/Products?$expand=Category` |
| **Truy vấn phức hợp** | `/odata/Products?$filter=Price gt 50&$orderby=Name&$top=5` |

---

## 🔐 Luồng xác thực (Authentication Flow)

1.  **Đăng nhập:** Người dùng nhập thông tin tại trang Login của `MyFE`.
2.  **Gửi yêu cầu:** `MyFE` gửi POST request tới `MyAPI/api/auth/login`.
3.  **Tạo Token:** `MyAPI` xác thực thông tin và cấp phát **JWT**.
4.  **Lưu trữ:** `MyFE` nhận token và lưu vào Cookie an toàn (HttpOnly Cookie: `MyAuthCookie`).
5.  **Gửi kèm Token:** Với các request cần quyền (ví dụ: Tạo sản phẩm), Frontend sẽ tự động lấy token và đính kèm vào Header `Authorization: Bearer <token>`.

---

## 📂 Cấu trúc thư mục

```text
daohd2003-myshopdaohd/
├── MyAPI/                  # Web API Project
│   ├── Controllers/        # Auth, Order, Product Controllers
│   └── appsettings.json    # Cấu hình DB Connection & JWT Secret
├── MyFE/                   # Razor Pages Client
│   ├── Pages/              # Giao diện (Login, List, Details)
│   └── wwwroot/            # Static assets (CSS, JS)
├── Services/               # Business Logic Layer
├── Repositories/           # Data Access Logic Layer
├── DataAccessObjects/      # EF Core Context & Config
└── BussinessObjects/       # Entities & DTOs
```

---
