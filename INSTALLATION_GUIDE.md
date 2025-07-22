# Hướng dẫn Cài đặt - AI Character Chat Platform

**Phiên bản**: 1.0.0  
**Ngày cập nhật**: 22/07/2025  
**Tác giả**: Manus AI

## Tổng quan

Tài liệu này cung cấp hướng dẫn chi tiết để cài đặt và cấu hình AI Character Chat Platform trên các hệ điều hành khác nhau. Ứng dụng hỗ trợ Windows, Linux và macOS với các phương thức cài đặt linh hoạt.

## Yêu cầu Hệ thống

### Yêu cầu Tối thiểu

- **Hệ điều hành**: Windows 10 (1903+), Ubuntu 18.04+, macOS 10.15+
- **RAM**: 4GB (khuyến nghị 8GB+)
- **Dung lượng ổ cứng**: 500MB trống
- **Kết nối Internet**: Cần thiết cho Google Gemini API
- **Độ phân giải màn hình**: 1024x768 (khuyến nghị 1920x1080+)

### Yêu cầu Phần mềm

- **.NET 8 Runtime** (tự động bao gồm trong self-contained builds)
- **Google Gemini API Key** (cần đăng ký tại Google AI Studio)

## Phương thức Cài đặt

### Phương thức 1: Sử dụng Pre-built Binaries (Khuyến nghị)

#### Windows

1. **Tải xuống**
   - Truy cập trang Releases trên GitHub repository
   - Tải file `AICharacterChat-win-x64.zip`
   - Giải nén vào thư mục mong muốn (ví dụ: `C:\Program Files\AICharacterChat`)

2. **Cài đặt**
   ```cmd
   # Giải nén file zip
   # Chạy AICharacterChat.UI.exe
   ```

3. **Tạo Shortcut** (Tùy chọn)
   - Click chuột phải vào `AICharacterChat.UI.exe`
   - Chọn "Create shortcut"
   - Di chuyển shortcut đến Desktop hoặc Start Menu

#### Linux (Ubuntu/Debian)

1. **Tải xuống và cài đặt**
   ```bash
   # Tải xuống
   wget https://github.com/your-repo/releases/download/v1.0.0/AICharacterChat-linux-x64.tar.gz
   
   # Giải nén
   tar -xzf AICharacterChat-linux-x64.tar.gz
   
   # Di chuyển đến thư mục ứng dụng
   sudo mv AICharacterChat /opt/
   
   # Tạo symbolic link
   sudo ln -s /opt/AICharacterChat/AICharacterChat.UI /usr/local/bin/aicharacterchat
   
   # Cấp quyền thực thi
   sudo chmod +x /opt/AICharacterChat/AICharacterChat.UI
   ```

2. **Tạo Desktop Entry**
   ```bash
   # Tạo file .desktop
   cat > ~/.local/share/applications/aicharacterchat.desktop << EOF
   [Desktop Entry]
   Name=AI Character Chat
   Comment=Chat with AI Characters
   Exec=/opt/AICharacterChat/AICharacterChat.UI
   Icon=/opt/AICharacterChat/icon.png
   Terminal=false
   Type=Application
   Categories=Office;Chat;
   EOF
   
   # Cập nhật desktop database
   update-desktop-database ~/.local/share/applications/
   ```

#### macOS

1. **Tải xuống**
   ```bash
   # Cho Intel Macs
   curl -L -o AICharacterChat-osx-x64.tar.gz https://github.com/your-repo/releases/download/v1.0.0/AICharacterChat-osx-x64.tar.gz
   
   # Cho Apple Silicon Macs
   curl -L -o AICharacterChat-osx-arm64.tar.gz https://github.com/your-repo/releases/download/v1.0.0/AICharacterChat-osx-arm64.tar.gz
   ```

2. **Cài đặt**
   ```bash
   # Giải nén
   tar -xzf AICharacterChat-osx-*.tar.gz
   
   # Di chuyển đến Applications
   mv AICharacterChat.app /Applications/
   
   # Cấp quyền thực thi (nếu cần)
   chmod +x /Applications/AICharacterChat.app/Contents/MacOS/AICharacterChat.UI
   ```

3. **Xử lý Gatekeeper** (nếu cần)
   ```bash
   # Nếu gặp cảnh báo bảo mật
   sudo xattr -rd com.apple.quarantine /Applications/AICharacterChat.app
   ```

### Phương thức 2: Build từ Source Code

#### Chuẩn bị Môi trường

1. **Cài đặt .NET 8 SDK**
   
   **Windows:**
   - Tải từ https://dotnet.microsoft.com/download/dotnet/8.0
   - Chạy installer và làm theo hướng dẫn
   
   **Linux (Ubuntu):**
   ```bash
   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0
   ```
   
   **macOS:**
   ```bash
   # Sử dụng Homebrew
   brew install dotnet
   ```

2. **Cài đặt Git**
   ```bash
   # Ubuntu/Debian
   sudo apt-get install git
   
   # macOS
   brew install git
   
   # Windows: Tải từ https://git-scm.com/
   ```

#### Build Process

1. **Clone Repository**
   ```bash
   git clone https://github.com/your-repo/AICharacterChatPlatform.git
   cd AICharacterChatPlatform
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build Solution**
   ```bash
   # Debug build
   dotnet build
   
   # Release build
   dotnet build --configuration Release
   ```

4. **Run Application**
   ```bash
   cd AICharacterChat.UI
   dotnet run
   ```

5. **Publish Self-Contained** (Tùy chọn)
   ```bash
   # Windows x64
   dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r win-x64 --self-contained true -o ./publish/win-x64
   
   # Linux x64
   dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64
   
   # macOS x64
   dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r osx-x64 --self-contained true -o ./publish/osx-x64
   
   # macOS ARM64
   dotnet publish AICharacterChat.UI/AICharacterChat.UI.csproj -c Release -r osx-arm64 --self-contained true -o ./publish/osx-arm64
   ```

## Cấu hình

### Cấu hình Google Gemini API

1. **Lấy API Key**
   - Truy cập https://makersuite.google.com/app/apikey
   - Đăng nhập với Google Account
   - Tạo API Key mới
   - Sao chép API Key

2. **Cấu hình trong Ứng dụng**
   
   **Cách 1: Chỉnh sửa appsettings.json**
   ```json
   {
     "GeminiApi": {
       "ApiKey": "YOUR_API_KEY_HERE",
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
   
   **Cách 2: Sử dụng Environment Variables**
   ```bash
   # Linux/macOS
   export GeminiApi__ApiKey="YOUR_API_KEY_HERE"
   
   # Windows
   set GeminiApi__ApiKey=YOUR_API_KEY_HERE
   ```

### Cấu hình Database

Ứng dụng sử dụng SQLite database được tạo tự động. Để tùy chỉnh:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=custom_database.db"
  }
}
```

### Cấu hình Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## Xác minh Cài đặt

### Kiểm tra Cơ bản

1. **Khởi động Ứng dụng**
   - Chạy ứng dụng từ shortcut hoặc command line
   - Kiểm tra giao diện hiển thị đúng
   - Không có error messages trong console

2. **Kiểm tra Database**
   - Tạo nhân vật mới
   - Kiểm tra nhân vật được lưu thành công
   - File `AICharacterChat.db` được tạo trong thư mục ứng dụng

3. **Kiểm tra API Connection**
   - Tạo cuộc hội thoại mới
   - Gửi tin nhắn test
   - Nhận phản hồi từ AI character

### Troubleshooting

#### Lỗi Thường Gặp

**1. "API Key not configured"**
```
Giải pháp:
- Kiểm tra API key trong appsettings.json
- Đảm bảo API key hợp lệ và có quyền truy cập
- Kiểm tra environment variables nếu sử dụng
```

**2. "Database connection failed"**
```
Giải pháp:
- Kiểm tra quyền ghi trong thư mục ứng dụng
- Xóa file database cũ nếu bị corrupt
- Kiểm tra connection string trong cấu hình
```

**3. "Application won't start"**
```
Giải pháp:
- Kiểm tra .NET runtime đã được cài đặt
- Chạy từ command line để xem error messages
- Kiểm tra file logs trong thư mục ứng dụng
```

**4. "UI không hiển thị đúng"**
```
Giải pháp:
- Cập nhật graphics drivers
- Kiểm tra độ phân giải màn hình
- Thử chạy với administrator privileges (Windows)
```

#### Debug Mode

Để chạy trong debug mode:

```bash
# Set environment variable
export ASPNETCORE_ENVIRONMENT=Development

# Hoặc chỉnh sửa appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

## Gỡ cài đặt

### Windows
1. Xóa thư mục cài đặt
2. Xóa shortcuts từ Desktop/Start Menu
3. Xóa registry entries (nếu có)

### Linux
```bash
# Xóa application files
sudo rm -rf /opt/AICharacterChat

# Xóa symbolic link
sudo rm /usr/local/bin/aicharacterchat

# Xóa desktop entry
rm ~/.local/share/applications/aicharacterchat.desktop
```

### macOS
```bash
# Xóa application
rm -rf /Applications/AICharacterChat.app

# Xóa user data (nếu muốn)
rm -rf ~/Library/Application\ Support/AICharacterChat
```

## Hỗ trợ

Nếu gặp vấn đề trong quá trình cài đặt:

1. Kiểm tra [FAQ](FAQ.md)
2. Tìm kiếm trong [Issues](https://github.com/your-repo/issues)
3. Tạo issue mới với thông tin chi tiết về lỗi
4. Liên hệ support team

---

**Tài liệu này được cập nhật thường xuyên. Vui lòng kiểm tra phiên bản mới nhất trên repository.**

