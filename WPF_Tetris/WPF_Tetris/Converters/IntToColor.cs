using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WPF_Tetris.Models;

namespace WPF_Tetris.Converters
{
    public class IntToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;

            switch (type)
            {
                case Constants.Block_background:
                    if (parameter != null)
                        return new SolidColorBrush(Colors.Transparent);
                    return new SolidColorBrush(Colors.Black);
                case Constants.Block_wall:
                    return new SolidColorBrush(Colors.DarkGray);
                case Constants.Block_1:
                    return new SolidColorBrush(Colors.Red);
                case Constants.Block_2:
                    return new SolidColorBrush(Colors.DeepPink);
                case Constants.Block_3:
                    return new SolidColorBrush(Colors.LawnGreen);
                case Constants.Block_4:
                    return new SolidColorBrush(Colors.Green);
                case Constants.Block_5:
                    return new SolidColorBrush(Colors.Blue);
                case Constants.Block_6:
                    return new SolidColorBrush(Colors.LightCoral);
                case Constants.Block_7:
                    return new SolidColorBrush(Colors.Yellow);
               case Constants.Block_8:
                   return new SolidColorBrush(Colors.SkyBlue);
               /*case Constants.Block_9:
                   return new SolidColorBrush(Colors.DarkRed);
                case Constants.Block_10:
                    return new SolidColorBrush(Colors.Indigo);*/
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
