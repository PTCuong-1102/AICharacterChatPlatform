using Avalonia.Controls;
using System.Collections.Specialized;
using AICharacterChat.UI.ViewModels;
using Avalonia;

namespace AICharacterChat.UI.Views
{
    public partial class ChatView : UserControl
    {
        private ChatViewModel? _currentViewModel;
        
        public ChatView()
        {
            InitializeComponent();
            
            // Subscribe to DataContext changes to hook up message collection change events
            this.PropertyChanged += OnDataContextChanged;
        }
        
        private void OnDataContextChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                // Unsubscribe from previous view model
                if (_currentViewModel != null)
                {
                    _currentViewModel.Messages.CollectionChanged -= OnMessagesCollectionChanged;
                }
                
                // Subscribe to new view model
                if (DataContext is ChatViewModel chatViewModel)
                {
                    _currentViewModel = chatViewModel;
                    chatViewModel.Messages.CollectionChanged += OnMessagesCollectionChanged;
                }
                else
                {
                    _currentViewModel = null;
                }
            }
        }
        
        private void OnMessagesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Auto-scroll to bottom when new messages are added
                ScrollToBottom();
            }
        }
        
        private void ScrollToBottom()
        {
            var listBox = this.FindControl<ListBox>("MessagesListBox");
            if (listBox?.Items != null && listBox.Items.Count > 0)
            {
                // Use Dispatcher to ensure scroll happens after UI update
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => 
                {
                    // Scroll to the last item in the list
                    var lastIndex = listBox.Items.Count - 1;
                    if (lastIndex >= 0)
                    {
                        listBox.ScrollIntoView(lastIndex);
                    }
                }, Avalonia.Threading.DispatcherPriority.Background);
            }
        }
    }
}

