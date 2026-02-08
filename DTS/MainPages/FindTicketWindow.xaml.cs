using DTS.Data;
using DTS.MainPages;
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
        private void FindButton_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CodeTextBox.Text))
            {
                ErrorsHighlight(CodeTextBox, CodeErrorBlock);
                return;
            }

            var db = DataBase.Instance;
            var ticket = db.GetTicketByCode(CodeTextBox.Text);

            if (ticket != null)
            {
                var window = new TicketView(ticket, false, 0);
                window.Show();

                this.Close(); 
            }
            else
            {
                ErrorsHighlight(CodeTextBox, CodeErrorBlock);
            }
        }


        private async void ErrorsHighlight(TextBox textBox, TextBlock textBlock)
        {
            var oldBrush = textBox.BorderBrush;
            var oldThickness = textBox.BorderThickness;

            textBlock.Visibility = Visibility.Visible;

            textBox.BorderBrush = Brushes.Red;
            textBox.BorderThickness = new Thickness(2);

            await Task.Delay(3000);

            textBox.BorderBrush = oldBrush;
            textBox.BorderThickness = oldThickness;

            textBlock.Visibility = Visibility.Collapsed;
        }


    }
}
