using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using AICharacterChat.UI.Services;

namespace AICharacterChat.UI.ViewModels;

/// <summary>
/// Main window ViewModel managing navigation and application state
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IThemeService _themeService;

    [ObservableProperty]
    private ViewModelBase? currentView;

    [ObservableProperty]
    private string currentViewTitle = "AI Character Chat Platform";

    [ObservableProperty]
    private bool isChatViewSelected = true;

    [ObservableProperty]
    private bool isCharacterManagementViewSelected;

    [ObservableProperty]
    private bool isDarkMode;

    [ObservableProperty]
    private string themeToggleText = "🌙";

    public ChatViewModel ChatViewModel { get; }
    public CharacterManagementViewModel CharacterManagementViewModel { get; }

    public MainWindowViewModel(
        ChatViewModel chatViewModel,
        CharacterManagementViewModel characterManagementViewModel,
        IThemeService themeService,
        ILogger<MainWindowViewModel> logger)
    {
        ChatViewModel = chatViewModel;
        CharacterManagementViewModel = characterManagementViewModel;
        _themeService = themeService;
        _logger = logger;

        // Set initial view
        CurrentView = ChatViewModel;
        CurrentViewTitle = "Trò chuyện";

        // Subscribe to theme changes
        _themeService.ThemeChanged += OnThemeChanged;
        UpdateThemeToggleText();
    }

    private void OnThemeChanged(object? sender, ThemeMode theme)
    {
        IsDarkMode = theme == ThemeMode.Dark;
        UpdateThemeToggleText();
    }

    private void UpdateThemeToggleText()
    {
        ThemeToggleText = IsDarkMode ? "☀️" : "🌙";
    }

    [RelayCommand]
    private async Task ShowChatViewAsync()
    {
        try
        {
            CurrentView = ChatViewModel;
            CurrentViewTitle = "Trò chuyện";
            IsChatViewSelected = true;
            IsCharacterManagementViewSelected = false;

            // Always refresh characters when switching to chat view
            // This ensures newly created characters appear in the chat
            await ChatViewModel.LoadCharactersCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error showing chat view");
        }
    }

    [RelayCommand]
    private async Task ShowCharacterManagementViewAsync()
    {
        try
        {
            CurrentView = CharacterManagementViewModel;
            CurrentViewTitle = "Quản lý nhân vật";
            IsChatViewSelected = false;
            IsCharacterManagementViewSelected = true;

            // Load characters if not already loaded
            if (!CharacterManagementViewModel.Characters.Any())
            {
                await CharacterManagementViewModel.LoadCharactersCommand.ExecuteAsync(null);
            }
            
            // Initialize in new character mode (no character selected)
            CharacterManagementViewModel.StartNewCharacterCommand.Execute(null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error showing character management view");
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        try
        {
            _themeService.ToggleTheme();
            _logger.LogInformation("Theme toggled to {Theme}", _themeService.CurrentTheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling theme");
        }
    }

    [RelayCommand]
    private async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Initializing application");
            
            // Load initial data
            await ChatViewModel.LoadCharactersCommand.ExecuteAsync(null);
            
            _logger.LogInformation("Application initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing application");
        }
    }
}
