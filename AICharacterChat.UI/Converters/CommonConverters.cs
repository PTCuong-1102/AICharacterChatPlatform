using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AICharacterChat.UI.Converters
{
    /// <summary>
    /// Converter from boolean to background brush for selection
    /// </summary>
    public class BooleanToBackgroundConverter : IValueConverter
    {
        public static readonly BooleanToBackgroundConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                return new SolidColorBrush(Color.Parse("#E3F2FD")); // Selected from theme
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter from object to boolean (null check)
    /// </summary>
    public class ObjectToBooleanConverter : IValueConverter
    {
        public static readonly ObjectToBooleanConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool result = value != null;
            
            // Check if we need to invert the result
            if (parameter?.ToString()?.Equals("Invert", StringComparison.OrdinalIgnoreCase) == true)
            {
                result = !result;
            }
            
            return result;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for message background color based on sender
    /// </summary>
    public class MessageBackgroundConverter : IValueConverter
    {
        public static readonly MessageBackgroundConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isFromUser)
            {
                return isFromUser 
                    ? new SolidColorBrush(Color.Parse("#1976D2"))  // User message - Primary blue
                    : new SolidColorBrush(Color.Parse("#F5F5F5")); // AI message - Surface variant
            }
            return new SolidColorBrush(Color.Parse("#F5F5F5"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for message text color based on sender
    /// </summary>
    public class MessageTextColorConverter : IValueConverter
    {
        public static readonly MessageTextColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isFromUser)
            {
                return isFromUser 
                    ? new SolidColorBrush(Colors.White)           // User message - white text on blue
                    : new SolidColorBrush(Color.Parse("#212121")); // AI message - dark text on light
            }
            return new SolidColorBrush(Color.Parse("#212121"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for message time color based on sender
    /// </summary>
    public class MessageTimeColorConverter : IValueConverter
    {
        public static readonly MessageTimeColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isFromUser)
            {
                return isFromUser 
                    ? new SolidColorBrush(Color.Parse("#E1F5FE"))  // User message - very light blue
                    : new SolidColorBrush(Color.Parse("#757575"));  // AI message - medium gray
            }
            return new SolidColorBrush(Color.Parse("#757575"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for message alignment based on sender
    /// </summary>
    public class MessageAlignmentConverter : IValueConverter
    {
        public static readonly MessageAlignmentConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isFromUser)
            {
                return isFromUser 
                    ? Avalonia.Layout.HorizontalAlignment.Right   // User message - right aligned
                    : Avalonia.Layout.HorizontalAlignment.Left;   // AI message - left aligned
            }
            return Avalonia.Layout.HorizontalAlignment.Left;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for edit mode header text
    /// </summary>
    public class EditModeHeaderConverter : IValueConverter
    {
        public static readonly EditModeHeaderConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isEditing)
            {
                return isEditing ? "Chỉnh sửa nhân vật" : "Thông tin nhân vật";
            }
            return "Thông tin nhân vật";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for save button background color based on CanSave state
    /// </summary>
    public class BooleanToSaveButtonBackgroundConverter : IValueConverter
    {
        public static readonly BooleanToSaveButtonBackgroundConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool canSave)
            {
                return canSave 
                    ? new SolidColorBrush(Color.Parse("#388E3C"))  // Bright green when enabled
                    : new SolidColorBrush(Color.Parse("#9E9E9E"));  // Gray when disabled
            }
            return new SolidColorBrush(Color.Parse("#9E9E9E"));
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

