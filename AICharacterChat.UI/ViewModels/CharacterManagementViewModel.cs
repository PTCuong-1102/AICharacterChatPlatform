using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using AICharacterChat.Business.Services.Interfaces;

namespace AICharacterChat.UI.ViewModels
{
    /// <summary>
    /// ViewModel for managing characters (CRUD operations)
    /// </summary>
    public partial class CharacterManagementViewModel : ViewModelBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILogger<CharacterManagementViewModel> _logger;

        [ObservableProperty]
        private ObservableCollection<CharacterViewModel> characters = new();

        [ObservableProperty]
        private CharacterViewModel? selectedCharacter;

        [ObservableProperty]
        private string characterName = string.Empty;

        [ObservableProperty]
        private string characterDescription = string.Empty;

        [ObservableProperty]
        private string systemPrompt = string.Empty;

        [ObservableProperty]
        private string avatarUrl = string.Empty;

        [ObservableProperty]
        private bool isEditing;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool isSaving;

        [ObservableProperty]
        private string statusMessage = "Sẵn sàng";

        [ObservableProperty]
        private string searchText = string.Empty;

        public CharacterManagementViewModel(
            ICharacterService characterService,
            ILogger<CharacterManagementViewModel> logger)
        {
            _characterService = characterService;
            _logger = logger;
        }

        [RelayCommand]
        private async Task LoadCharactersAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tải danh sách nhân vật...";

                var charactersData = await _characterService.GetAllCharactersAsync();
                
                Characters.Clear();
                foreach (var character in charactersData)
                {
                    Characters.Add(new CharacterViewModel(character));
                }

                StatusMessage = $"Đã tải {Characters.Count} nhân vật";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading characters");
                StatusMessage = "Lỗi khi tải danh sách nhân vật";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchCharactersAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Đang tìm kiếm...";

                var charactersData = string.IsNullOrWhiteSpace(SearchText)
                    ? await _characterService.GetAllCharactersAsync()
                    : await _characterService.SearchCharactersAsync(SearchText);
                
                Characters.Clear();
                foreach (var character in charactersData)
                {
                    Characters.Add(new CharacterViewModel(character));
                }

                StatusMessage = $"Tìm thấy {Characters.Count} nhân vật";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching characters");
                StatusMessage = "Lỗi khi tìm kiếm nhân vật";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void SelectCharacter(CharacterViewModel character)
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.IsSelected = false;
            }

            SelectedCharacter = character;
            character.IsSelected = true;

            // Load character data into form
            CharacterName = character.Name;
            CharacterDescription = character.Description ?? string.Empty;
            SystemPrompt = character.SystemPrompt;
            AvatarUrl = character.AvatarUrl ?? string.Empty;
        }

        [RelayCommand]
        private void StartNewCharacter()
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.IsSelected = false;
            }

            SelectedCharacter = null;
            IsEditing = false;
            ClearForm();
            StatusMessage = "Nhân vật mới đã sẵn sàng";
        }

        [RelayCommand]
        private void StartEditCharacter()
        {
            if (SelectedCharacter == null) return;

            IsEditing = true;
            StatusMessage = "Chế độ chỉnh sửa";
        }

        [RelayCommand]
        private void CancelEdit()
        {
            IsEditing = false;
            
            if (SelectedCharacter != null)
            {
                // Restore original values
                CharacterName = SelectedCharacter.Name;
                CharacterDescription = SelectedCharacter.Description ?? string.Empty;
                SystemPrompt = SelectedCharacter.SystemPrompt;
                AvatarUrl = SelectedCharacter.AvatarUrl ?? string.Empty;
            }
            else
            {
                ClearForm();
            }

            StatusMessage = "Đã hủy chỉnh sửa";
        }

        [RelayCommand]
        private async Task SaveCharacterAsync()
        {
            if (string.IsNullOrWhiteSpace(CharacterName) || string.IsNullOrWhiteSpace(SystemPrompt))
            {
                StatusMessage = "Vui lòng nhập tên và system prompt";
                return;
            }

            try
            {
                IsSaving = true;
                StatusMessage = "Đang lưu nhân vật...";

                if (IsEditing && SelectedCharacter != null)
                {
                    // Update existing character
                    var updatedCharacter = await _characterService.UpdateCharacterAsync(
                        SelectedCharacter.CharacterId,
                        CharacterName.Trim(),
                        CharacterDescription.Trim(),
                        SystemPrompt.Trim(),
                        string.IsNullOrWhiteSpace(AvatarUrl) ? null : AvatarUrl.Trim());

                    if (updatedCharacter != null)
                    {
                        SelectedCharacter.UpdateFromModel(updatedCharacter);
                        StatusMessage = "Đã cập nhật nhân vật thành công";
                    }
                }
                else
                {
                    // Create new character
                    var newCharacter = await _characterService.CreateCharacterAsync(
                        CharacterName.Trim(),
                        CharacterDescription.Trim(),
                        SystemPrompt.Trim(),
                        string.IsNullOrWhiteSpace(AvatarUrl) ? null : AvatarUrl.Trim());

                    var newCharacterViewModel = new CharacterViewModel(newCharacter);
                    Characters.Add(newCharacterViewModel);
                    SelectCharacter(newCharacterViewModel);

                    StatusMessage = "Đã tạo nhân vật mới thành công";
                }

                IsEditing = false;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error saving character");
                StatusMessage = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving character");
                StatusMessage = "Lỗi khi lưu nhân vật";
            }
            finally
            {
                IsSaving = false;
            }
        }

        [RelayCommand]
        private async Task DeleteCharacterAsync(CharacterViewModel character)
        {
            try
            {
                var success = await _characterService.DeleteCharacterAsync(character.CharacterId);
                
                if (success)
                {
                    Characters.Remove(character);
                    
                    if (SelectedCharacter == character)
                    {
                        SelectedCharacter = null;
                        ClearForm();
                    }

                    StatusMessage = "Đã xóa nhân vật thành công";
                }
                else
                {
                    StatusMessage = "Không thể xóa nhân vật";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting character {CharacterId}", character.CharacterId);
                StatusMessage = "Lỗi khi xóa nhân vật";
            }
        }

        [RelayCommand]
        private async Task ValidateSystemPromptAsync()
        {
            if (string.IsNullOrWhiteSpace(SystemPrompt))
            {
                StatusMessage = "Vui lòng nhập system prompt";
                return;
            }

            try
            {
                StatusMessage = "Đang kiểm tra system prompt...";
                
                var isValid = await _characterService.ValidateSystemPromptAsync(SystemPrompt);
                
                StatusMessage = isValid 
                    ? "System prompt hợp lệ" 
                    : "System prompt có thể không hoạt động tốt";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating system prompt");
                StatusMessage = "Lỗi khi kiểm tra system prompt";
            }
        }

        private void ClearForm()
        {
            CharacterName = string.Empty;
            CharacterDescription = string.Empty;
            SystemPrompt = string.Empty;
            AvatarUrl = string.Empty;
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(CharacterName) && 
                              !string.IsNullOrWhiteSpace(SystemPrompt) && 
                              !IsSaving;

        public bool CanDelete => SelectedCharacter != null && !IsSaving;
        public bool CanEdit => SelectedCharacter != null && !IsEditing && !IsSaving;
    }
}

