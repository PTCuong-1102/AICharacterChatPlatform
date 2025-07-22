using System;
using CommunityToolkit.Mvvm.ComponentModel;
using AICharacterChat.Data.Models;

namespace AICharacterChat.UI.ViewModels
{
    /// <summary>
    /// ViewModel for displaying character information
    /// </summary>
    public partial class CharacterViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int characterId;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string systemPrompt = string.Empty;

        [ObservableProperty]
        private string? avatarUrl;

        [ObservableProperty]
        private DateTime createdAt;

        [ObservableProperty]
        private bool isSelected;

        public CharacterViewModel()
        {
        }

        public CharacterViewModel(Character character)
        {
            CharacterId = character.CharacterId;
            Name = character.Name;
            Description = character.Description;
            SystemPrompt = character.SystemPrompt;
            AvatarUrl = character.AvatarUrl;
            CreatedAt = character.CreatedAt;
        }

        public Character ToModel()
        {
            return new Character
            {
                CharacterId = CharacterId,
                Name = Name,
                Description = Description,
                SystemPrompt = SystemPrompt,
                AvatarUrl = AvatarUrl,
                CreatedAt = CreatedAt
            };
        }

        public void UpdateFromModel(Character character)
        {
            CharacterId = character.CharacterId;
            Name = character.Name;
            Description = character.Description;
            SystemPrompt = character.SystemPrompt;
            AvatarUrl = character.AvatarUrl;
            CreatedAt = character.CreatedAt;
        }
    }
}

