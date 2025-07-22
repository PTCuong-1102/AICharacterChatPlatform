using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AICharacterChat.UI.ViewModels;
using AICharacterChat.UI.Views;
using AICharacterChat.Data;
using AICharacterChat.Data.Repositories.Interfaces;
using AICharacterChat.Data.Repositories;
using AICharacterChat.Business.Services.Interfaces;
using AICharacterChat.Business.Services;
using AICharacterChat.Business.Configuration;
using AICharacterChat.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace AICharacterChat.UI;

public partial class App : Application
{
    private IHost? _host;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Setup dependency injection
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Configuration
                services.Configure<GeminiApiConfiguration>(configuration.GetSection(GeminiApiConfiguration.SectionName));

                // Database
                services.AddDbContext<ChatDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? 
                                        "Data Source=AICharacterChat.db"));

                // Repositories
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<ICharacterRepository, CharacterRepository>();
                services.AddScoped<IConversationRepository, ConversationRepository>();
                services.AddScoped<IMessageRepository, MessageRepository>();

                // Services
                services.AddHttpClient<IGeminiApiService, GeminiApiService>();
                services.AddScoped<IChatService, ChatService>();
                services.AddScoped<ICharacterService, CharacterService>();
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ICharacterRefreshService, CharacterRefreshService>();

                // ViewModels
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<ChatViewModel>();
                services.AddTransient<CharacterManagementViewModel>();

                // Logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            })
            .Build();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            // Get MainWindowViewModel from DI container
            var mainWindowViewModel = _host.Services.GetRequiredService<MainWindowViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel,
            };

            // Initialize the application
            _ = Task.Run(async () =>
            {
                try
                {
                    // Initialize database
                    using (var scope = _host.Services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
                        await context.Database.EnsureCreatedAsync();
                        
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<App>>();
                        logger.LogInformation("Database initialized successfully");
                    }
                    
                    await mainWindowViewModel.InitializeCommand.ExecuteAsync(null);
                }
                catch (Exception ex)
                {
                    var logger = _host.Services.GetRequiredService<ILogger<App>>();
                    logger.LogError(ex, "Error during application initialization");
                }
            });
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}