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
                    return Brushes.Green;
                case TimeWarpState.Working:
                    return Brushes.Red;
                case TimeWarpState.None:
                    return Brushes.Silver;
            }

            throw new ArgumentOutOfRangeException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}