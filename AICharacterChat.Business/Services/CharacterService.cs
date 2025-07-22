using Microsoft.Extensions.Logging;
using AICharacterChat.Data.Models;
using AICharacterChat.Data.Repositories.Interfaces;
using AICharacterChat.Business.Services.Interfaces;

namespace AICharacterChat.Business.Services
{
    /// <summary>
    /// Character service implementation managing character operations
    /// </summary>
    public class CharacterService : ICharacterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeminiApiService _geminiApiService;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(
            IUnitOfWork unitOfWork,
            IGeminiApiService geminiApiService,
            ILogger<CharacterService> logger)
        {
            _unitOfWork = unitOfWork;
            _geminiApiService = geminiApiService;
            _logger = logger;
        }

        public async Task<IEnumerable<Character>> GetAllCharactersAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Characters.GetAllAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(int characterId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Characters.GetByIdAsync(characterId);
        }

        public async Task<Character?> GetCharacterWithConversationsAsync(int characterId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Characters.GetWithConversationsAsync(characterId);
        }

        public async Task<Character> CreateCharacterAsync(
            string name,
            string description,
            string systemPrompt,
            string? avatarUrl = null,
            CancellationToken cancellationToken = default)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Character name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(systemPrompt))
                throw new ArgumentException("System prompt is required", nameof(systemPrompt));

            // Check if name is available
            if (!await IsCharacterNameAvailableAsync(name, cancellationToken: cancellationToken))
                throw new ArgumentException($"Character name '{name}' is already taken", nameof(name));

            // Validate system prompt
            if (!await ValidateSystemPromptAsync(systemPrompt, cancellationToken))
                _logger.LogWarning("System prompt validation failed for character '{Name}'", name);

            var character = new Character
            {
                Name = name.Trim(),
                Description = description?.Trim(),
                SystemPrompt = systemPrompt.Trim(),
                AvatarUrl = avatarUrl?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Characters.AddAsync(character);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new character '{Name}' with ID {CharacterId}", character.Name, character.CharacterId);
            return character;
        }

        public async Task<Character?> UpdateCharacterAsync(
            int characterId,
            string name,
            string description,
            string systemPrompt,
            string? avatarUrl = null,
            CancellationToken cancellationToken = default)
        {
            var character = await _unitOfWork.Characters.GetByIdAsync(characterId);
            if (character == null)
                return null;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Character name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(systemPrompt))
                throw new ArgumentException("System prompt is required", nameof(systemPrompt));

            // Check if name is available (excluding current character)
            if (!await IsCharacterNameAvailableAsync(name, characterId, cancellationToken))
                throw new ArgumentException($"Character name '{name}' is already taken", nameof(name));

            // Validate system prompt
            if (!await ValidateSystemPromptAsync(systemPrompt, cancellationToken))
                _logger.LogWarning("System prompt validation failed for character '{Name}'", name);

            character.Name = name.Trim();
            character.Description = description?.Trim();
            character.SystemPrompt = systemPrompt.Trim();
            character.AvatarUrl = avatarUrl?.Trim();

            _unitOfWork.Characters.Update(character);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Updated character '{Name}' with ID {CharacterId}", character.Name, character.CharacterId);
            return character;
        }

        public async Task<bool> DeleteCharacterAsync(int characterId, CancellationToken cancellationToken = default)
        {
            var character = await _unitOfWork.Characters.GetByIdAsync(characterId);
            if (character == null)
                return false;

            _unitOfWork.Characters.Delete(character);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Deleted character '{Name}' with ID {CharacterId}", character.Name, character.CharacterId);
            return true;
        }

        public async Task<IEnumerable<Character>> SearchCharactersAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Characters.SearchAsync(searchTerm);
        }

        public async Task<IEnumerable<Character>> GetRecentCharactersAsync(int count = 10, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Characters.GetRecentAsync(count);
        }

        public async Task<bool> IsCharacterNameAvailableAsync(string name, int? excludeCharacterId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var existingCharacter = await _unitOfWork.Characters.GetByNameAsync(name.Trim());
            
            if (existingCharacter == null)
                return true;

            // If we're updating a character, exclude it from the check
            return excludeCharacterId.HasValue && existingCharacter.CharacterId == excludeCharacterId.Value;
        }

        public async Task<bool> ValidateSystemPromptAsync(string systemPrompt, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(systemPrompt))
                    return false;

                // Test the system prompt with a simple message
                var testResponse = await _geminiApiService.GenerateResponseAsync(
                    "Xin chào",
                    systemPrompt,
                    null,
                    cancellationToken);

                return !string.IsNullOrEmpty(testResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating system prompt");
                return false;
            }
        }
    }
}

