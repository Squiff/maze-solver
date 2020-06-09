using MazeSolver.ViewModel;
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
    /// Convert CellTypeVM Enum to UI friendly string
    /// </summary>
    class CellTypeVMConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CellTypeVM)value)
            {
                case CellTypeVM.Path:
                    return "Path";
                case CellTypeVM.Wall:
                    return "Wall";
                case CellTypeVM.Start:
                    return "Start";
                case CellTypeVM.Finish:
                    return "Finish";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
