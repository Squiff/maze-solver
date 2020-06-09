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
using MazeSolver.ViewModel;

namespace MazeSolver.View
{
    /// <summary>
    /// Interaction logic for Cell.xaml
    /// </summary>
    public partial class Cell : UserControl
    {
        public Cell()
        {
            InitializeComponent();
        }


        #region SetCellTypeCommand
        /// <summary>
        /// Command to be executed when cell is clicked or cell is entered with mousedown
        /// 
        /// </summary>
        public ICommand SetCellTypeCommand
        {
            get { return (ICommand)GetValue(SetCellTypeCommandProperty); }
            set { SetValue(SetCellTypeCommandProperty, value); }
        }

        public static readonly DependencyProperty SetCellTypeCommandProperty =
            DependencyProperty.Register("SetCellTypeCommand", typeof(ICommand), typeof(Cell), new PropertyMetadata(null));

        #endregion

        private void RunCommand(ICommand command)
        {
            if (command.CanExecute(null))
                command.Execute(null);
        }


        /// <summary>
        /// Run the SetCellTypeCommand when left clicked
        /// </summary>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RunCommand(SetCellTypeCommand);
        }

        /// <summary>
        /// Run the SetCellTypeCommand when cursor enters the cell and left button is pressed
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            RunCommand(SetCellTypeCommand);
        }
    }
}
