using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Tetris_WPF.Models;

namespace Tetris_WPF.Converters
{
    public class IntToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;

            switch(type)
            {
                case Constants.Block_background:
                    if (parameter != null)
                        return new SolidColorBrush(Colors.Transparent);
                    return new SolidColorBrush(Colors.Black);                    
                case Constants.Block_wall:
                    return new SolidColorBrush(Colors.Gray);
                case Constants.Block_1:
                    return new SolidColorBrush(Colors.Purple);
                case Constants.Block_2:
                    return new SolidColorBrush(Colors.Yellow);
                case Constants.Block_3:
                    return new SolidColorBrush(Colors.Blue);
                case Constants.Block_4:
                    return new SolidColorBrush(Colors.Red);
                case Constants.Block_5:
                    return new SolidColorBrush(Colors.Green);
                case Constants.Block_6:
                    return new SolidColorBrush(Colors.HotPink);
                case Constants.Block_7:
                    return new SolidColorBrush(Colors.MintCream);
            }
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
