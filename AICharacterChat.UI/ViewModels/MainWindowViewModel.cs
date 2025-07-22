using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AICharacterChat.UI.ViewModels;

/// <summary>
/// Main window ViewModel managing navigation and application state
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> _logger;

    [ObservableProperty]
    private ViewModelBase? currentView;

    [ObservableProperty]
    private string currentViewTitle = "AI Character Chat Platform";

    [ObservableProperty]
    private bool isChatViewSelected = true;

    [ObservableProperty]
    private bool isCharacterManagementViewSelected;

    public ChatViewModel ChatViewModel { get; }
    public CharacterManagementViewModel CharacterManagementViewModel { get; }

    public MainWindowViewModel(
        ChatViewModel chatViewModel,
        CharacterManagementViewModel characterManagementViewModel,
        ILogger<MainWindowViewModel> logger)
    {
        ChatViewModel = chatViewModel;
        CharacterManagementViewModel = characterManagementViewModel;
        _logger = logger;

        // Set initial view
        CurrentView = ChatViewModel;
        CurrentViewTitle = "Trò chuyện";
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

            // Load characters if not already loaded
            if (!ChatViewModel.Characters.Any())
            {
                await ChatViewModel.LoadCharactersCommand.ExecuteAsync(null);
            }
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error showing character management view");
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
