using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace BiznesApp.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (Application.Current == null) return Colors.Gray;

            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "nowe" => Application.Current.Resources["Primary"],
                    "w realizacji" => Application.Current.Resources["Warning"],
                    "zakończone" => Application.Current.Resources["Success"],
                    "anulowane" => Application.Current.Resources["Error"],
                    "wysłana" => Application.Current.Resources["BusinessBlue"],
                    "zaakceptowana" => Application.Current.Resources["Success"],
                    "odrzucona" => Application.Current.Resources["Error"],
                    _ => Application.Current.Resources["Gray500"]
                };
            }
            return Application.Current.Resources["Gray500"];
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
} 