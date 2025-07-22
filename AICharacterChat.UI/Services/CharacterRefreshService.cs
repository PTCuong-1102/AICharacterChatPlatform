using System;

namespace AICharacterChat.UI.Services
{
    /// <summary>
    /// Implementation of character refresh notification service
    /// </summary>
    public class CharacterRefreshService : ICharacterRefreshService
    {
        /// <summary>
        /// Event fired when characters should be refreshed
        /// </summary>
        public event EventHandler? CharactersChanged;

        /// <summary>
        /// Notify all subscribers that characters have changed
        /// </summary>
        public void NotifyCharactersChanged()
        {
            CharactersChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
