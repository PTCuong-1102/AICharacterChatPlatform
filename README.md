# AI Character Chat Platform

Nền tảng Chat với Nhân vật AI sử dụng AvaloniaUI, .NET 8, Entity Framework Core, SQL Server và Google Gemini 2.0 Flash API.

## Tính năng chính

- **Quản lý nhân vật AI**: Tạo, chỉnh sửa và xóa các nhân vật AI với tính cách riêng biệt
- **Chat thông minh**: Trò chuyện với các nhân vật AI được hỗ trợ bởi Google Gemini 2.0 Flash API
- **Lịch sử hội thoại**: Lưu trữ và quản lý các cuộc hội thoại
- **Giao diện hiện đại**: Sử dụng AvaloniaUI với MVVM pattern
- **Đa nền tảng**: Hỗ trợ Windows, Linux và macOS

## Kiến trúc hệ thống

### Cấu trúc dự án

```
AICharacterChatPlatform/
├── AICharacterChat.Data/          # Tầng dữ liệu
│   ├── Models/                    # Entity models
│   ├── Repositories/              # Repository pattern
│   └── ChatDbContext.cs           # Entity Framework DbContext
├── AICharacterChat.Business/      # Tầng nghiệp vụ
│   ├── Services/                  # Business services
│   ├── Configuration/             # Cấu hình
│   └── Models/                    # Business models
└── AICharacterChat.UI/            # Tầng giao diện
    ├── Views/                     # XAML views
    ├── ViewModels/                # MVVM ViewModels
    └── Converters/                # Data converters
```

### Công nghệ sử dụng

- **.NET 8**: Framework chính
- **AvaloniaUI**: Cross-platform UI framework
- **Entity Framework Core**: ORM cho database
- **SQL Server**: Database engine với LocalDB support
- **Google Gemini 2.0 Flash**: AI language model tiên tiến
- **MVVM Pattern**: Kiến trúc giao diện
- **Dependency Injection**: Quản lý dependencies

## Yêu cầu hệ thống

- **.NET 8 SDK** - Framework runtime và development tools
- **SQL Server LocalDB** - Được cài đặt tự động với Visual Studio hoặc SQL Server Express
- **Google Gemini API Key** - Để truy cập AI services
- **Windows, Linux hoặc macOS** - Hỗ trợ đa nền tảng

## Cài đặt và chạy

### 1. Clone repository

```bash
git clone <repository-url>
cd AICharacterChatPlatform
```

### 2. Cấu hình Database và API Key

#### 2.1. Cấu hình SQL Server LocalDB
Database sẽ được tạo tự động khi chạy ứng dụng. Connection string mặc định:
```
Server=(localdb)\mssqllocaldb;Database=AICharacterChatDb;Trusted_Connection=true;MultipleActiveResultSets=true
```

#### 2.2. Cấu hình Gemini API Key
Chỉnh sửa file `AICharacterChat.UI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AICharacterChatDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "GeminiApi": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
    "Model": "gemini-2.0-flash",
    "MaxTokens": 2048,
    "Temperature": 0.7,
    "TopP": 0.8,
    "TopK": 40,
    "TimeoutSeconds": 30,
    "MaxContextMessages": 10
  }
}
```

**Lưu ý**: Để lấy Gemini API Key, truy cập [Google AI Studio](https://makersuite.google.com/app/apikey)

### 3. Restore packages

```bash
dotnet restore
```

### 4. Build solution

```bash
dotnet build
```

### 5. Áp dụng Database Migration (nếu cần)

```bash
cd AICharacterChat.Data
dotnet ef database update
```

### 6. Chạy ứng dụng

```bash
cd AICharacterChat.UI
dotnet run
```

**Lưu ý**: Khi chạy lần đầu, ứng dụng sẽ tự động tạo database và seed dữ liệu mẫu (2 nhân vật AI mặc định).

## Publish ứng dụng

Sử dụng script publish để tạo executable cho các nền tảng khác nhau:

```bash
./publish.sh
```

Các file executable sẽ được tạo trong thư mục `publish/`:
- `publish/win-x64/` - Windows 64-bit
- `publish/linux-x64/` - Linux 64-bit  
- `publish/osx-x64/` - macOS Intel
- `publish/osx-arm64/` - macOS Apple Silicon

## Hướng dẫn sử dụng

### Quản lý nhân vật

1. Mở tab "Quản lý nhân vật"
2. Nhấn "Nhân vật mới" để tạo nhân vật mới
3. Điền thông tin:
   - **Tên nhân vật**: Tên hiển thị
   - **Mô tả**: Mô tả ngắn về nhân vật
   - **Avatar URL**: Link ảnh đại diện (tùy chọn)
   - **System Prompt**: Định nghĩa tính cách và cách hành xử
4. Nhấn "Tạo nhân vật" để tạo mới hoặc "Cập nhật" để chỉnh sửa

### Trò chuyện

1. Mở tab "Trò chuyện"
2. Chọn nhân vật từ danh sách bên trái
3. Nhấn "+" để tạo cuộc hội thoại mới hoặc chọn cuộc hội thoại có sẵn
4. Nhập tin nhắn và nhấn "Gửi" hoặc Enter

## Cấu hình nâng cao

### Database (SQL Server)

Ứng dụng sử dụng **SQL Server LocalDB** với database tên `AICharacterChatDb`. Database sẽ được tạo tự động khi chạy ứng dụng lần đầu.

#### Connection String mặc định:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AICharacterChatDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### Sử dụng SQL Server khác:
Để kết nối đến SQL Server khác, thay đổi connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AICharacterChatDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Hoặc với SQL Authentication:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AICharacterChatDb;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### Gemini 2.0 Flash API

Ứng dụng sử dụng **Google Gemini 2.0 Flash** - model AI tiên tiến nhất hiện tại. Cấu hình chi tiết trong `appsettings.json`:

```json
{
  "GeminiApi": {
    "ApiKey": "your-gemini-api-key",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
    "Model": "gemini-2.0-flash",
    "MaxTokens": 2048,
    "Temperature": 0.7,
    "TopP": 0.8,
    "TopK": 40,
    "TimeoutSeconds": 30,
    "MaxContextMessages": 10
  }
}
```

#### Các tham số cấu hình:
- **ApiKey**: API key từ Google AI Studio
- **Model**: `gemini-2.0-flash` (khuyến nghị) hoặc `gemini-1.5-flash`
- **Temperature**: 0.0-2.0 (độ sáng tạo của AI)
- **MaxTokens**: Số token tối đa cho phản hồi
- **MaxContextMessages**: Số tin nhắn context giữ lại cho cuộc trò chuyện

## Troubleshooting

### Lỗi API Key

Nếu gặp lỗi liên quan đến API key:
1. Kiểm tra API key trong `appsettings.json`
2. Đảm bảo API key có quyền truy cập Gemini API
3. Kiểm tra kết nối internet

### Lỗi Database

Nếu gặp lỗi database:
1. **Lỗi LocalDB**: Cài đặt SQL Server Express LocalDB
   - Download từ [Microsoft SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
   - Hoặc cài đặt qua Visual Studio Installer
2. **Lỗi Migration**: Chạy lại migration
   ```bash
   cd AICharacterChat.Data
   dotnet ef database drop --force
   dotnet ef database update
   ```
3. **Lỗi Connection**: Kiểm tra connection string trong `appsettings.json`

### Lỗi UI

Nếu giao diện không hiển thị đúng:
1. Đảm bảo đã cài đặt .NET 8 runtime
2. Kiểm tra log trong console

## Đóng góp

1. Fork repository
2. Tạo feature branch
3. Commit changes
4. Push to branch
5. Tạo Pull Request

## License

MIT License - xem file LICENSE để biết thêm chi tiết.

## Liên hệ

Nếu có vấn đề hoặc đề xuất, vui lòng tạo issue trên GitHub repository.

