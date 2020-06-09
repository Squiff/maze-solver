using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using MazeSolver.ViewModel;
//using MazeSolver.Model;

namespace MazeSolver.View
{
    /// <summary>
    /// Interaction logic for Maze.xaml
    /// </summary>
    public partial class Maze : UserControl
    {
        private MazeViewModel _mazeViewModel;

        public Maze()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Set the UI size for the maze and listen for property changed events
        /// </summary>
        private void Maze_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mazeViewModel = (MazeViewModel)this.DataContext;

            _mazeViewModel.PropertyChanged += MazeVM_PropertyChanged;
            SetMazeSize(this.ActualHeight, this.ActualWidth);
        }

        /// <summary>
        /// Helper function to set the size of the itemsControl so it fills available space while maintaining a square ratio for cells
        /// </summary>
        /// <param name="availableHeight">available height to expand</param>
        /// <param name="availableWidth">available width to expand</param>
        private void SetMazeSize(double availableHeight, double availableWidth)
        {
            double availableCellWidth = availableWidth / _mazeViewModel.Columns;
            double availableCellHeight = availableHeight / _mazeViewModel.Rows;

            // Take the min value so entire Maze element fits (akin to stretch = "uniform")
            double cellSize = Math.Min(availableCellWidth, availableCellHeight);
            // resize the itemsControl
            RootItemsControl.Height = cellSize * _mazeViewModel.Rows;
            RootItemsControl.Width = cellSize * _mazeViewModel.Columns;
        }

        /// <summary>
        /// size changed handler to ensure each cell of the maze is kept square after resizing
        /// </summary>
        private void Maze_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_mazeViewModel != null)
                SetMazeSize(e.NewSize.Height, e.NewSize.Width);

        }

        /// <summary>
        /// property changed handler for changes to datacontext properties
        /// </summary>
        public void MazeVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Maze")
                SetMazeSize(this.ActualHeight, this.ActualWidth);
        }

        
    }
}
