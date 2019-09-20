using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray.Wpf
{
    public class TimeWarpStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeWarpState pomodoroState = (TimeWarpState)value;

            switch (pomodoroState)
            {
                case TimeWarpState.Resting:
                    return new SolidColorBrush(Colors.Green);;
                case TimeWarpState.Working:
                    return new SolidColorBrush(Colors.Red);;
                case TimeWarpState.None:
                    return new SolidColorBrush(Colors.DarkGray);
            }

            throw new ArgumentOutOfRangeException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}