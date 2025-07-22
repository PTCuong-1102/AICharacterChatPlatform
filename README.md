# AI Character Chat Platform

Nền tảng Chat với Nhân vật AI sử dụng AvaloniaUI, .NET 8, Entity Framework Core và Google Gemini API.

## Tính năng chính

- **Quản lý nhân vật AI**: Tạo, chỉnh sửa và xóa các nhân vật AI với tính cách riêng biệt
- **Chat thông minh**: Trò chuyện với các nhân vật AI được hỗ trợ bởi Google Gemini API
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
- **SQLite**: Database engine
- **Google Gemini API**: AI language model
- **MVVM Pattern**: Kiến trúc giao diện
- **Dependency Injection**: Quản lý dependencies

## Yêu cầu hệ thống

- .NET 8 SDK
- Google Gemini API Key

## Cài đặt và chạy

### 1. Clone repository

```bash
git clone <repository-url>
cd AICharacterChatPlatform
```

### 2. Cấu hình API Key

Chỉnh sửa file `AICharacterChat.UI/appsettings.json`:

```json
{
  "GeminiApi": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  }
}
```

### 3. Restore packages

```bash
dotnet restore
```

### 4. Build solution

```bash
dotnet build
```

### 5. Chạy ứng dụng

```bash
cd AICharacterChat.UI
dotnet run
```

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
4. Nhấn "Lưu" để hoàn tất

### Trò chuyện

1. Mở tab "Trò chuyện"
2. Chọn nhân vật từ danh sách bên trái
3. Nhấn "+" để tạo cuộc hội thoại mới hoặc chọn cuộc hội thoại có sẵn
4. Nhập tin nhắn và nhấn "Gửi" hoặc Enter

## Cấu hình nâng cao

### Database

Ứng dụng sử dụng SQLite database được tạo tự động tại `AICharacterChat.db`. Để thay đổi connection string, chỉnh sửa `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=your_database.db"
  }
}
```

### Gemini API

Cấu hình chi tiết cho Gemini API trong `appsettings.json`:

```json
{
  "GeminiApi": {
    "ApiKey": "your-api-key",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
    "Model": "gemini-pro",
    "MaxTokens": 2048,
    "Temperature": 0.7,
    "TopP": 0.8,
    "TopK": 40,
    "TimeoutSeconds": 30,
    "MaxContextMessages": 10
  }
}
```

## Troubleshooting

### Lỗi API Key

Nếu gặp lỗi liên quan đến API key:
1. Kiểm tra API key trong `appsettings.json`
2. Đảm bảo API key có quyền truy cập Gemini API
3. Kiểm tra kết nối internet

### Lỗi Database

Nếu gặp lỗi database:
1. Xóa file `AICharacterChat.db`
2. Chạy lại ứng dụng để tạo database mới

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

