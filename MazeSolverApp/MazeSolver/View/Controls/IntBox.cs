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

namespace MazeSolver.View
{
    /// <summary>
    /// Custom Control to suppress input for non-integers, spaces and leading zeros
    /// </summary>
    public class IntBox : TextBox
    {
        public IntBox()
        {
            PreviewKeyDown += OnPreviewKeyDown;
            PreviewTextInput += OnPreviewTextInput;
        }


        /// <summary>
        /// prevent space from being entered (Not handled by PreviewTextInput)
        /// </summary>
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        /// <summary>
        /// prevent non numeric characters being entered
        /// </summary>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            Regex notNumber = new Regex("[^0-9]");

            if (notNumber.IsMatch(e.Text))
                e.Handled = true;
            else if (textbox.CaretIndex == 0 && e.Text == "0")
                e.Handled = true;
        }
    }
}