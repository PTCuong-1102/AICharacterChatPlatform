using System;

namespace AICharacterChat.UI.Services
{
    /// <summary>
    /// Service for notifying ViewModels when characters need to be refreshed
    /// </summary>
    public interface ICharacterRefreshService
    {
        /// <summary>
        /// Event fired when characters should be refreshed
        /// </summary>
        event EventHandler? CharactersChanged;
        
        /// <summary>
        /// Notify all subscribers that characters have changed
        /// </summary>
        void NotifyCharactersChanged();
    }
}
