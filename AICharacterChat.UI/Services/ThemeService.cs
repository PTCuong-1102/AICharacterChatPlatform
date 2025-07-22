using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls.ApplicationLifetimes;
using AICharacterChat.UI.Converters;

namespace AICharacterChat.UI.Services
{
    public enum ThemeMode
    {
        Light,
        Dark
    }

    public interface IThemeService
    {
        ThemeMode CurrentTheme { get; }
        event EventHandler<ThemeMode>? ThemeChanged;
        void SetTheme(ThemeMode theme);
        void ToggleTheme();
    }

    public class ThemeService : IThemeService
    {
        private ThemeMode _currentTheme = ThemeMode.Light;

        public ThemeMode CurrentTheme => _currentTheme;

        public event EventHandler<ThemeMode>? ThemeChanged;

        public void SetTheme(ThemeMode theme)
        {
            if (_currentTheme == theme) return;

            _currentTheme = theme;
            ApplyTheme(theme);
            ThemeChanged?.Invoke(this, theme);
        }

        public void ToggleTheme()
        {
            var newTheme = _currentTheme == ThemeMode.Light ? ThemeMode.Dark : ThemeMode.Light;
            SetTheme(newTheme);
        }

        private void ApplyTheme(ThemeMode theme)
        {
            if (Application.Current?.Resources == null) return;

            // Direct resource assignment
            if (theme == ThemeMode.Dark)
            {
                ApplyDarkTheme(Application.Current.Resources);
            }
            else
            {
                ApplyLightTheme(Application.Current.Resources);
            }
            
            // Force invalidation
            InvalidateVisualTree();
        }

        private void InvalidateVisualTree()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow?.InvalidateVisual();
            }
        }

        private void ApplyDarkTheme(Avalonia.Controls.IResourceNode resources)
        {
            var resourceDict = resources as Avalonia.Controls.IResourceDictionary;
            if (resourceDict == null) return;

            // Primary Colors
            resourceDict["PrimaryBrush"] = new SolidColorBrush(Color.Parse("#2196F3"));
            resourceDict["PrimaryLightBrush"] = new SolidColorBrush(Color.Parse("#64B5F6"));
            resourceDict["PrimaryDarkBrush"] = new SolidColorBrush(Color.Parse("#1565C0"));

            // Surface Colors
            resourceDict["SurfaceBrush"] = new SolidColorBrush(Color.Parse("#1E1E1E"));
            resourceDict["SurfaceVariantBrush"] = new SolidColorBrush(Color.Parse("#2D2D2D"));
            resourceDict["SurfaceTintBrush"] = new SolidColorBrush(Color.Parse("#252525"));

            // Background Colors
            resourceDict["BackgroundBrush"] = new SolidColorBrush(Color.Parse("#121212"));
            resourceDict["BackgroundSecondaryBrush"] = new SolidColorBrush(Color.Parse("#1A1A1A"));

            // Text Colors
            resourceDict["OnSurfaceBrush"] = new SolidColorBrush(Color.Parse("#FFFFFF"));
            resourceDict["OnSurfaceVariantBrush"] = new SolidColorBrush(Color.Parse("#E0E0E0"));
            resourceDict["OnSurfaceMutedBrush"] = new SolidColorBrush(Color.Parse("#BDBDBD"));
            resourceDict["OnPrimaryBrush"] = new SolidColorBrush(Color.Parse("#000000"));

            // Border Colors
            resourceDict["OutlineBrush"] = new SolidColorBrush(Color.Parse("#404040"));
            resourceDict["OutlineVariantBrush"] = new SolidColorBrush(Color.Parse("#333333"));
        }

        private void ApplyLightTheme(Avalonia.Controls.IResourceNode resources)
        {
            var resourceDict = resources as Avalonia.Controls.IResourceDictionary;
            if (resourceDict == null) return;

            // Primary Colors
            resourceDict["PrimaryBrush"] = new SolidColorBrush(Color.Parse("#1976D2"));
            resourceDict["PrimaryLightBrush"] = new SolidColorBrush(Color.Parse("#42A5F5"));
            resourceDict["PrimaryDarkBrush"] = new SolidColorBrush(Color.Parse("#0D47A1"));

            // Surface Colors
            resourceDict["SurfaceBrush"] = new SolidColorBrush(Color.Parse("#FFFFFF"));
            resourceDict["SurfaceVariantBrush"] = new SolidColorBrush(Color.Parse("#F5F5F5"));
            resourceDict["SurfaceTintBrush"] = new SolidColorBrush(Color.Parse("#FAFAFA"));

            // Background Colors
            resourceDict["BackgroundBrush"] = new SolidColorBrush(Color.Parse("#FAFAFA"));
            resourceDict["BackgroundSecondaryBrush"] = new SolidColorBrush(Color.Parse("#F5F5F5"));

            // Text Colors
            resourceDict["OnSurfaceBrush"] = new SolidColorBrush(Color.Parse("#212121"));
            resourceDict["OnSurfaceVariantBrush"] = new SolidColorBrush(Color.Parse("#424242"));
            resourceDict["OnSurfaceMutedBrush"] = new SolidColorBrush(Color.Parse("#757575"));
            resourceDict["OnPrimaryBrush"] = new SolidColorBrush(Color.Parse("#FFFFFF"));

            // Border Colors
            resourceDict["OutlineBrush"] = new SolidColorBrush(Color.Parse("#E0E0E0"));
            resourceDict["OutlineVariantBrush"] = new SolidColorBrush(Color.Parse("#EEEEEE"));
        }
    }
}