using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MazeSolver.View
{
    /// <summary>
    /// Convert bool to 'Start' or 'Stop'
    /// </summary>
    class IsRunningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsRunning = (bool)value;

            return IsRunning ? "Stop" : "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
