using MazeSolver.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace MazeSolver.View
{
    /// <summary>
    /// Convert CellStatus to Colour
    /// </summary>
    class CellColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellStatus cellStatus = (CellStatus)value;

            // switch CellStatus, return colour from application resources
            switch (cellStatus)
            {
                case CellStatus.Path:
                    return Application.Current.FindResource("PathCell");
                case CellStatus.PathVisited:
                    return Application.Current.FindResource("VisitedCell");
                case CellStatus.Wall:
                    return Application.Current.FindResource("WallCell");
                case CellStatus.Start:
                    return Application.Current.FindResource("StartCell");
                case CellStatus.Finish:
                    return Application.Current.FindResource("FinishCell");
                case CellStatus.SolutionPath:
                    return Application.Current.FindResource("SolutionPathCell");
                default:
                    return Application.Current.FindResource("PathCell");
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
