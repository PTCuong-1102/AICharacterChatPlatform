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
                return new SolidColorBrush(Color.Parse("#E8F4FD"));
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
            return value != null;
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
                    ? new SolidColorBrush(Color.Parse("#3498DB"))  // User message - blue
                    : new SolidColorBrush(Color.Parse("#ECF0F1")); // AI message - light gray
            }
            return new SolidColorBrush(Color.Parse("#ECF0F1"));
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
                    ? new SolidColorBrush(Colors.White)           // User message - white text
                    : new SolidColorBrush(Color.Parse("#2C3E50")); // AI message - dark text
            }
            return new SolidColorBrush(Color.Parse("#2C3E50"));
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
                    ? new SolidColorBrush(Color.Parse("#BDC3C7"))  // User message - light gray
                    : new SolidColorBrush(Color.Parse("#7F8C8D"));  // AI message - medium gray
            }
            return new SolidColorBrush(Color.Parse("#7F8C8D"));
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
}

