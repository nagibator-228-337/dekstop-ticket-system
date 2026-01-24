using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DTS
{
    /// <summary>
    /// Логика взаимодействия для FindTicketWindow.xaml
    /// </summary>
    public partial class FindTicketWindow : Window
    {
        public FindTicketWindow()
        {
            InitializeComponent();

            CodeTextBox.GotFocus += (s, e) => UpdatePlacholder(CodeTextBox, CodePlaceholder);
            CodeTextBox.LostFocus += (s, e) => UpdatePlacholder(CodeTextBox, CodePlaceholder);
            CodeTextBox.TextChanged += (s, e) => UpdatePlacholder(CodeTextBox, CodePlaceholder);

        }

        private void UpdatePlacholder(TextBox box, TextBlock placeholder)
        {
            if (box.IsFocused || !string.IsNullOrWhiteSpace(box.Text))
            {
                placeholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                placeholder.Visibility = Visibility.Visible;
            }
        }
    }
}
