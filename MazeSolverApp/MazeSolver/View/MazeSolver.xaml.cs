using MazeSolver.View;
using MazeSolver.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MazeSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MazeViewModel();
        }


        private void Rows_Error(object sender, ValidationErrorEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            ValidationBase x = (ValidationBase)textBox.DataContext;

            var y = (BindingExpression)e.Error.BindingInError;
            string propertyname = y.ParentBinding.Path.Path;

            x.AddError(propertyname, e.Error.ErrorContent.ToString());
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help H = new Help();
            H.Show();
        }

        /// <summary>
        /// When main window is closed, exit the application
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
