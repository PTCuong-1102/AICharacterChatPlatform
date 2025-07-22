# Tài liệu Kỹ thuật - AI Character Chat Platform

**Tác giả**: Manus AI  
**Ngày tạo**: 22/07/2025  
**Phiên bản**: 1.0.0

## Tóm tắt điều hành

AI Character Chat Platform là một ứng dụng desktop đa nền tảng được phát triển để tạo ra trải nghiệm trò chuyện tương tác với các nhân vật AI được cá nhân hóa. Dự án này sử dụng công nghệ tiên tiến bao gồm .NET 8, AvaloniaUI, Entity Framework Core và Google Gemini API để cung cấp một nền tảng mạnh mẽ, linh hoạt và dễ sử dụng.

Ứng dụng được thiết kế theo kiến trúc phân lớp rõ ràng với pattern MVVM, đảm bảo tính bảo trì cao và khả năng mở rộng trong tương lai. Với giao diện người dùng hiện đại và trực quan, người dùng có thể dễ dàng tạo, quản lý và tương tác với các nhân vật AI có tính cách riêng biệt.




## Kiến trúc Hệ thống

### Tổng quan Kiến trúc

Hệ thống được thiết kế theo mô hình kiến trúc phân lớp (Layered Architecture) với ba tầng chính: Presentation Layer (Tầng Giao diện), Business Logic Layer (Tầng Nghiệp vụ), và Data Access Layer (Tầng Truy cập Dữ liệu). Kiến trúc này đảm bảo tính tách biệt rõ ràng giữa các thành phần, giúp dễ dàng bảo trì, kiểm thử và mở rộng hệ thống.

Tầng Giao diện sử dụng AvaloniaUI framework với pattern MVVM (Model-View-ViewModel) để tạo ra giao diện người dùng đa nền tảng. Pattern này cho phép tách biệt hoàn toàn logic giao diện khỏi logic nghiệp vụ, đồng thời hỗ trợ data binding mạnh mẽ và testing hiệu quả.

Tầng Nghiệp vụ chứa toàn bộ logic xử lý của ứng dụng, bao gồm việc tích hợp với Google Gemini API để xử lý các yêu cầu chat AI. Tầng này được thiết kế theo Service Pattern, với các service được inject thông qua Dependency Injection container, đảm bảo loose coupling và high cohesion.

Tầng Truy cập Dữ liệu sử dụng Entity Framework Core với Repository Pattern và Unit of Work Pattern để quản lý dữ liệu. Điều này cung cấp một abstraction layer mạnh mẽ cho việc truy cập database, đồng thời hỗ trợ transaction management và caching hiệu quả.

### Công nghệ Core

**.NET 8 Framework** được chọn làm nền tảng chính do tính ổn định, hiệu suất cao và hỗ trợ đa nền tảng tuyệt vời. Framework này cung cấp runtime hiệu quả, garbage collection tối ưu và hỗ trợ async/await pattern mạnh mẽ cho việc xử lý các tác vụ bất đồng bộ như API calls.

**AvaloniaUI** được sử dụng thay vì WPF hoặc WinUI để đảm bảo ứng dụng có thể chạy trên Windows, Linux và macOS mà không cần thay đổi code. Framework này cung cấp XAML-based UI development tương tự WPF nhưng với khả năng cross-platform native.

**Entity Framework Core** được chọn làm ORM chính do khả năng tích hợp tốt với .NET ecosystem, hỗ trợ Code First approach và migration system mạnh mẽ. EF Core cũng cung cấp LINQ support và change tracking tự động, giúp đơn giản hóa việc làm việc với database.

**SQLite** được sử dụng làm database engine do tính portable, không cần cài đặt server riêng biệt và phù hợp với ứng dụng desktop. SQLite cung cấp ACID compliance và hiệu suất tốt cho các ứng dụng có quy mô vừa và nhỏ.

### Patterns và Principles

Hệ thống tuân thủ các design patterns và principles quan trọng để đảm bảo code quality và maintainability. **SOLID Principles** được áp dụng xuyên suốt, với Single Responsibility Principle được thể hiện qua việc tách biệt rõ ràng các concerns, Open/Closed Principle thông qua interface-based design, và Dependency Inversion Principle thông qua Dependency Injection.

**Repository Pattern** được implement để tạo abstraction layer giữa business logic và data access logic. Pattern này cho phép dễ dàng thay đổi data source mà không ảnh hưởng đến business logic, đồng thời hỗ trợ unit testing hiệu quả thông qua mock objects.

**Unit of Work Pattern** được sử dụng để quản lý transactions và đảm bảo data consistency. Pattern này cho phép group multiple repository operations thành một transaction duy nhất, đảm bảo atomicity của các operations phức tạp.

**MVVM Pattern** trong presentation layer đảm bảo separation of concerns giữa UI và business logic. ViewModels act như intermediary layer, handling UI logic và data binding, trong khi Views chỉ chịu trách nhiệm về presentation. Pattern này cũng hỗ trợ testability tốt cho UI components.


## Cấu trúc Dự án Chi tiết

### AICharacterChat.Data Project

Data layer project chứa toàn bộ logic liên quan đến data access và database operations. Project này được tổ chức theo cấu trúc rõ ràng với các thư mục chuyên biệt cho từng loại component.

**Models Directory** chứa các Entity classes đại diện cho database tables. Các models được thiết kế theo Entity Framework conventions với proper navigation properties và data annotations. Character model đại diện cho thông tin nhân vật AI, bao gồm Name, Description, SystemPrompt và AvatarUrl. Conversation model quản lý thông tin cuộc hội thoại với foreign key reference đến Character. Message model lưu trữ nội dung tin nhắn với metadata như timestamp và sender information.

**Repositories Directory** implement Repository Pattern với generic base repository và specialized repositories cho từng entity. IGenericRepository interface định nghĩa các operations cơ bản như CRUD operations, while specialized interfaces như ICharacterRepository extend base interface với domain-specific methods. Concrete implementations handle actual database operations thông qua Entity Framework DbContext.

**ChatDbContext** class kế thừa từ EF Core DbContext và định nghĩa database schema thông qua DbSet properties và Fluent API configurations. Context này handle database connections, change tracking và transaction management. OnModelCreating method được override để configure entity relationships, constraints và indexes.

### AICharacterChat.Business Project

Business logic layer chứa toàn bộ application logic và external service integrations. Layer này được thiết kế để independent với presentation layer và data layer, đảm bảo business rules có thể được test và maintain một cách riêng biệt.

**Services Directory** chứa các business service classes implement domain logic. IChatService interface định nghĩa methods cho chat operations như SendMessageAsync, GetConversationHistoryAsync và StartNewConversationAsync. ChatService implementation handle conversation flow, message validation và integration với AI service.

**IGeminiApiService** interface abstract việc communication với Google Gemini API, cho phép dễ dàng thay đổi AI provider trong tương lai. GeminiApiService implementation handle HTTP requests, response parsing và error handling cho Gemini API calls. Service này cũng implement retry logic và timeout handling để đảm bảo reliability.

**Configuration Directory** chứa configuration classes cho external services. GeminiApiConfiguration class define các properties như ApiKey, BaseUrl, Model parameters và timeout settings. Configuration này được bind từ appsettings.json thông qua Options pattern.

### AICharacterChat.UI Project

Presentation layer sử dụng AvaloniaUI framework với MVVM pattern để tạo cross-platform desktop application. Project này được organize theo View-ViewModel pairs với supporting infrastructure cho data binding và UI operations.

**ViewModels Directory** chứa các ViewModel classes implement INotifyPropertyChanged interface thông qua CommunityToolkit.Mvvm library. MainWindowViewModel act như root ViewModel, managing navigation giữa các views và global application state. ChatViewModel handle chat interface logic, managing character selection, conversation history và message sending. CharacterManagementViewModel handle CRUD operations cho characters với form validation và error handling.

**Views Directory** chứa XAML files định nghĩa UI layout và styling. MainWindow.axaml define overall application layout với navigation controls và content area. ChatView.axaml implement three-panel layout với character list, conversation list và chat interface. CharacterManagementView.axaml provide form-based interface cho character management với validation feedback.

**Converters Directory** chứa value converters cho data binding operations. Các converters handle việc convert giữa business objects và UI representations, như converting boolean values thành colors hoặc visibility states.

## Database Design

### Entity Relationship Model

Database schema được thiết kế để support core functionality của chat application với proper normalization và referential integrity. Schema bao gồm ba main entities với clear relationships và appropriate constraints.

**Characters Table** serve như master table cho AI character definitions. Table này chứa Id (Primary Key), Name (unique constraint), Description, SystemPrompt (required field định nghĩa AI behavior), AvatarUrl (optional), và audit fields như CreatedAt và UpdatedAt. SystemPrompt field đặc biệt quan trọng vì nó define personality và behavior của AI character.

**Conversations Table** represent individual chat sessions giữa user và specific character. Table có Id (Primary Key), CharacterId (Foreign Key reference đến Characters), Title (auto-generated hoặc user-defined), CreatedAt timestamp, và LastMessageAt để support sorting và filtering. Relationship với Characters table là many-to-one, cho phép một character có multiple conversations.

**Messages Table** store individual messages trong conversations. Schema include Id (Primary Key), ConversationId (Foreign Key), Content (message text), IsFromUser (boolean flag), SenderName, Timestamp, và optional metadata fields. Relationship với Conversations table là many-to-one, tạo hierarchical structure: Character -> Conversations -> Messages.

### Indexing Strategy

Database performance được optimize thông qua strategic indexing. Primary keys tự động có clustered indexes, while foreign keys có non-clustered indexes để support join operations. Additional indexes được tạo trên frequently queried fields như Characters.Name và Messages.Timestamp.

Composite indexes được consider cho common query patterns, như (ConversationId, Timestamp) trên Messages table để support efficient conversation history retrieval. Index maintenance strategy được design để balance query performance với insert/update overhead.

### Data Integrity và Constraints

Referential integrity được enforce thông qua foreign key constraints với appropriate cascade behaviors. Character deletion cascade đến related conversations và messages để maintain data consistency. Check constraints ensure data validity, như non-empty SystemPrompt và valid timestamp ranges.

Unique constraints prevent duplicate character names và ensure business rule compliance. Nullable fields được carefully designed để reflect business requirements, với required fields như Character.Name và Character.SystemPrompt marked as non-nullable.


## API Integration và External Services

### Google Gemini API Integration

Tích hợp với Google Gemini API được implement thông qua dedicated service layer để abstract API complexity và provide clean interface cho business logic. GeminiApiService class handle toàn bộ HTTP communication với Gemini endpoints, including request formatting, response parsing và error handling.

API requests được construct theo Gemini API specification với proper headers, authentication và payload formatting. Request payload include system prompt (từ character definition), conversation history (để maintain context), và user message. Response parsing extract AI-generated content và handle various response formats từ Gemini API.

**Authentication và Authorization** được handle thông qua API key mechanism. API key được store trong configuration và inject vào service thông qua Options pattern. Service implement secure header construction với proper Bearer token formatting. Error handling include specific logic cho authentication failures, rate limiting và API quota exceeded scenarios.

**Context Management** là critical aspect của AI chat implementation. Service maintain conversation context bằng cách include recent message history trong API requests. Context window được limit để avoid exceeding API token limits while maintaining conversation coherence. Oldest messages được remove khi context window full, ensuring optimal balance giữa context preservation và API efficiency.

**Rate Limiting và Retry Logic** được implement để handle API limitations và network issues. Service include exponential backoff retry mechanism cho transient failures, với configurable retry counts và delay intervals. Rate limiting được handle thông qua request queuing và throttling mechanisms.

### HTTP Client Configuration

HttpClient được configure với appropriate timeouts, connection pooling và error handling policies. Client được register trong DI container với singleton lifetime để optimize connection reuse. Custom HttpClientHandler được implement để add logging, request/response interception và custom error handling.

**Timeout Configuration** include both connection timeout và request timeout để prevent hanging requests. Timeout values được make configurable thông qua appsettings để allow environment-specific tuning. Cancellation tokens được propagate throughout async call chain để support request cancellation.

**SSL/TLS Configuration** ensure secure communication với external APIs. Certificate validation được enable với proper chain validation và hostname verification. Custom certificate handling được implement cho development environments nếu cần.

## Security Considerations

### Data Protection

Sensitive data protection được implement ở multiple layers để ensure comprehensive security. API keys và connection strings được store trong configuration với encryption at rest. Application implement secure configuration loading với support cho environment variables và secure key stores.

**Local Data Encryption** được consider cho sensitive conversation data. While SQLite database không inherently encrypted, application có thể extend để support database encryption thông qua SQLCipher hoặc similar solutions. User data được treat như sensitive information và handle accordingly.

**Memory Protection** include secure string handling cho API keys và sensitive configuration. Sensitive strings được clear từ memory sau use khi possible. Application avoid logging sensitive information và implement proper sanitization cho debug output.

### Input Validation và Sanitization

Comprehensive input validation được implement ở multiple layers để prevent injection attacks và data corruption. User inputs được validate both client-side và server-side (trong business layer) để ensure data integrity.

**SQL Injection Prevention** được achieve thông qua Entity Framework parameterized queries. EF Core automatically handle parameter sanitization và prevent SQL injection attacks. Raw SQL queries được avoid hoặc carefully parameterized khi necessary.

**XSS Prevention** trong UI layer được handle thông qua proper data binding và output encoding. AvaloniaUI framework provide built-in protection against XSS trong data binding scenarios. User-generated content được sanitize trước khi display.

**API Input Validation** include validation của message content, character definitions và configuration parameters. Business layer implement comprehensive validation rules với proper error messages và user feedback.

### Error Handling và Logging

Comprehensive error handling strategy được implement để provide good user experience while maintaining security. Errors được categorize thành user errors, system errors và security-related errors với appropriate handling cho each category.

**Logging Strategy** include structured logging với appropriate log levels và sensitive data filtering. Application logs được configure để avoid logging sensitive information như API keys hoặc personal data. Log rotation và retention policies được implement để manage log file sizes.

**Error Disclosure** được carefully manage để avoid information leakage. User-facing error messages được sanitize để remove technical details có thể expose system internals. Detailed error information được log cho debugging purposes nhưng không expose đến end users.

**Exception Handling** include global exception handlers ở application level và specific exception handling trong critical paths. Unhandled exceptions được log với full stack traces cho debugging while presenting user-friendly error messages.


## Performance Optimization

### Database Performance

Database performance được optimize thông qua multiple strategies bao gồm efficient querying, proper indexing và connection management. Entity Framework Core được configure với appropriate tracking behaviors và query optimization settings.

**Query Optimization** include sử dụng appropriate EF Core methods như AsNoTracking() cho read-only queries, Include() cho eager loading của related data, và projection queries để minimize data transfer. Complex queries được analyze và optimize thông qua query execution plans và performance profiling.

**Connection Pooling** được enable để minimize connection overhead. EF Core connection pooling được configure với appropriate pool sizes và connection lifetime settings. Database connections được properly dispose để avoid connection leaks và resource exhaustion.

**Caching Strategy** được implement ở multiple levels. In-memory caching được sử dụng cho frequently accessed data như character definitions và recent conversations. Cache invalidation strategies ensure data consistency while maximizing cache hit rates.

**Lazy Loading vs Eager Loading** được carefully balance dựa trên usage patterns. Related data được load efficiently thông qua explicit Include statements hoặc projection queries. N+1 query problems được identify và resolve thông qua proper query design.

### UI Performance

User interface performance được optimize để provide smooth user experience across different platforms và hardware configurations. AvaloniaUI performance best practices được follow để minimize UI thread blocking và optimize rendering performance.

**Data Binding Optimization** include efficient converter implementations và proper binding modes. Two-way binding được sử dụng only khi necessary, với one-way binding preferred cho read-only scenarios. Collection binding được optimize thông qua ObservableCollection và proper change notifications.

**UI Virtualization** được implement cho large data sets như conversation history và character lists. Virtual scrolling ensure smooth performance even với thousands of items. UI controls được optimize để minimize memory footprint và rendering overhead.

**Async Operations** được properly implement để avoid UI thread blocking. Long-running operations như API calls được execute trên background threads với proper progress reporting và cancellation support. UI updates được marshal back đến UI thread using appropriate dispatching mechanisms.

**Memory Management** include proper disposal của resources, event handler cleanup và weak reference patterns khi appropriate. Memory leaks được prevent thông qua careful resource management và proper object lifecycle handling.

### API Performance

External API performance được optimize thông qua efficient request patterns, caching strategies và connection reuse. HTTP client configuration được tune để maximize throughput while respecting API rate limits.

**Request Batching** được consider cho scenarios where multiple API calls có thể được combined. Request deduplication prevent duplicate API calls cho same content. Response caching được implement cho cacheable content với appropriate cache expiration policies.

**Connection Reuse** được maximize thông qua proper HttpClient configuration. Keep-alive connections được enable để reduce connection establishment overhead. Connection pooling settings được tune dựa trên expected load patterns.

**Compression** được enable cho HTTP requests và responses để minimize bandwidth usage. Gzip compression được support cho both request và response payloads khi API supports it.

## Deployment và Distribution

### Build Configuration

Build process được configure để support multiple target platforms với optimized output cho each platform. MSBuild configurations include Release builds với appropriate optimizations enabled và Debug builds với full debugging information.

**Platform-Specific Builds** được generate cho Windows (x64), Linux (x64), macOS (x64), và macOS (ARM64). Each build được optimize cho target platform với appropriate runtime configurations và native dependencies.

**Self-Contained Deployments** được prefer để minimize deployment dependencies. Applications được publish với .NET runtime included, eliminating need cho separate .NET installation trên target machines. Trimming được enable để reduce deployment size while maintaining functionality.

**Build Automation** được implement thông qua build scripts và CI/CD pipelines. Automated builds ensure consistent output across different environments và reduce manual deployment errors. Build verification include automated testing và quality checks.

### Packaging Strategy

Application packaging được design để provide easy installation và distribution across target platforms. Platform-specific packaging formats được sử dụng để integrate well với each operating system.

**Windows Packaging** include MSI installers với proper Windows integration. Start menu shortcuts, file associations và uninstall support được include. Windows-specific features như notification integration được leverage khi available.

**Linux Packaging** support multiple distribution formats including AppImage, Snap packages và traditional package managers. Desktop integration include .desktop files và icon installation. Package dependencies được properly declare để ensure smooth installation.

**macOS Packaging** include proper .app bundle creation với code signing và notarization support. macOS-specific features như dock integration và native menu bars được implement. Distribution thông qua Mac App Store được consider cho wider reach.

### Configuration Management

Application configuration được design để support different deployment environments với appropriate defaults và override mechanisms. Configuration sources include appsettings files, environment variables và command-line arguments.

**Environment-Specific Configuration** được support thông qua multiple appsettings files (Development, Production, etc.). Sensitive configuration như API keys được handle thông qua secure configuration providers và environment variables.

**Configuration Validation** ensure required settings được provide và have valid values. Application startup include configuration validation với clear error messages cho missing hoặc invalid settings. Default values được provide cho optional settings.

**Runtime Configuration Updates** được consider cho settings có thể safely change without application restart. Configuration change notifications được implement để update services khi configuration changes.

### Monitoring và Diagnostics

Production monitoring capabilities được include để support troubleshooting và performance analysis. Logging, metrics collection và health checks được implement để provide visibility into application behavior.

**Application Logging** include structured logging với appropriate log levels và contextual information. Log aggregation và analysis tools được support thông qua standard logging formats. Sensitive information được filter từ logs để maintain security.

**Performance Metrics** collection include response times, error rates và resource utilization. Metrics được expose thông qua standard formats để integrate với monitoring tools. Custom metrics được implement cho business-specific monitoring requirements.

**Health Checks** provide application status information cho monitoring systems. Health checks include database connectivity, external API availability và critical service status. Health check endpoints được implement để support automated monitoring.


## Testing Strategy

### Unit Testing Framework

Comprehensive unit testing strategy được implement để ensure code quality và prevent regressions. Testing framework sử dụng xUnit.net với supporting libraries như Moq cho mocking và FluentAssertions cho readable assertions.

**Business Logic Testing** focus vào testing service classes và business rules. Services được test in isolation với mocked dependencies để ensure pure unit testing. Test cases cover normal flows, edge cases và error conditions với comprehensive assertion coverage.

**Repository Testing** include testing của data access logic với in-memory database providers. EF Core InMemory provider được sử dụng để create isolated test environments. Database operations được test để ensure proper CRUD functionality và relationship handling.

**ViewModel Testing** cover UI logic testing với mocked service dependencies. Command execution, property change notifications và validation logic được thoroughly test. UI-specific logic được separate từ view code để enable effective testing.

### Integration Testing

Integration testing verify proper interaction giữa different system components. Test environments được setup với real database connections và external service integrations để test end-to-end functionality.

**Database Integration Tests** use test databases với known data sets để verify complex queries và transaction handling. Migration testing ensure database schema changes được properly applied. Performance testing identify slow queries và optimization opportunities.

**API Integration Tests** verify external service communication với proper error handling và retry logic. Mock API servers được sử dụng để simulate various response scenarios including errors và timeouts. Contract testing ensure API integration remains compatible với service changes.

**UI Integration Tests** verify proper interaction giữa ViewModels và Views. Data binding, command execution và navigation flows được test để ensure proper UI behavior. Automated UI testing tools được consider cho comprehensive UI coverage.

### Test Data Management

Test data management strategy ensure consistent và reliable test execution. Test databases được populate với known data sets để enable predictable test outcomes. Data cleanup strategies ensure test isolation và prevent test interference.

**Test Data Builders** được implement để create test objects với fluent APIs. Builder pattern enable easy test data creation với appropriate defaults và customization options. Shared test data được manage để avoid duplication while maintaining test independence.

**Database Seeding** cho integration tests include representative data sets covering various scenarios. Seed data được version controlled và automatically applied during test setup. Data anonymization được apply khi using production-like data cho testing.

## Future Enhancements

### Scalability Improvements

Future scalability enhancements được plan để support larger user bases và more complex usage patterns. Architecture được design với scalability in mind, enabling future enhancements without major restructuring.

**Multi-User Support** có thể được add để enable shared character libraries và collaborative features. User authentication và authorization systems có thể được integrate để support multi-tenant scenarios. Data isolation strategies ensure proper user data separation.

**Cloud Integration** opportunities include cloud-based character storage, conversation synchronization across devices, và cloud-based AI processing. Cloud storage providers như Azure Blob Storage hoặc AWS S3 có thể được integrate cho asset storage.

**Performance Scaling** enhancements include database optimization cho larger datasets, caching layers cho frequently accessed data, và background processing cho non-critical operations. Connection pooling và resource management có thể được further optimize cho high-load scenarios.

### Feature Enhancements

Additional features được consider để enhance user experience và expand application capabilities. Feature roadmap include both user-facing improvements và technical enhancements.

**Advanced AI Features** include support cho multiple AI providers, custom model fine-tuning, và advanced conversation management. Voice input/output capabilities có thể được add để create more immersive experiences. Image generation và processing features có thể enhance character interactions.

**Export/Import Capabilities** enable users để backup và share character definitions và conversations. Standard formats như JSON hoặc XML có thể được support cho interoperability. Batch operations enable efficient data management cho power users.

**Plugin Architecture** có thể được implement để enable third-party extensions và customizations. Plugin APIs enable developers để add custom features, AI providers, và UI components. Plugin marketplace có thể facilitate community-driven enhancements.

### Technical Debt và Refactoring

Ongoing technical debt management ensure codebase remains maintainable và extensible. Regular refactoring cycles identify improvement opportunities và implement best practices.

**Code Quality Improvements** include static analysis tool integration, code coverage improvements, và performance profiling. Automated code quality checks ensure consistent coding standards và identify potential issues early.

**Architecture Evolution** opportunities include microservices migration cho complex deployments, event-driven architecture cho better scalability, và domain-driven design principles cho complex business logic.

**Technology Updates** include regular framework updates, security patch applications, và adoption của new technologies. Migration strategies ensure smooth transitions while maintaining backward compatibility.

## Conclusion

AI Character Chat Platform represent một comprehensive solution cho AI-powered conversational experiences với robust architecture, comprehensive security measures, và excellent user experience. Technical implementation demonstrate best practices trong modern .NET development với proper separation of concerns, comprehensive testing, và scalable design.

Architecture choices enable future enhancements while maintaining current functionality và performance. Security considerations ensure user data protection và system integrity. Performance optimizations provide smooth user experience across different platforms và usage scenarios.

Development process follow industry best practices với comprehensive documentation, automated testing, và proper deployment strategies. Code quality measures ensure maintainable codebase với clear structure và good separation of concerns.

Future enhancement opportunities provide clear roadmap cho continued development và feature expansion. Technical foundation support scalability improvements và new feature additions without major architectural changes.

Project demonstrate successful integration của multiple technologies và frameworks để create cohesive, functional application. Implementation serve như good example của modern desktop application development với cross-platform support và cloud service integration.

---

**Tài liệu này được tạo bởi Manus AI vào ngày 22/07/2025. Phiên bản 1.0.0**

