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
    public partial class CreateTicketWindow : Window
    {
        public CreateTicketWindow()
        {
            InitializeComponent();

            //placeholder logic
            SubjectTextBox.GotFocus += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);
            SubjectTextBox.LostFocus += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);
            SubjectTextBox.TextChanged += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);

            ThemeTextBox.GotFocus += (s, e) => UpdatePlacholder(ThemeTextBox, ThemeTextPlaceholder);
            ThemeTextBox.LostFocus += (s, e) => UpdatePlacholder(ThemeTextBox, ThemeTextPlaceholder);
            ThemeTextBox.TextChanged += (s, e) => UpdatePlacholder(ThemeTextBox, ThemeTextPlaceholder);

            EmailTextBox.GotFocus += (s, e) => UpdatePlacholder(EmailTextBox, EmailPlaceholder);
            EmailTextBox.LostFocus += (s, e) => UpdatePlacholder(EmailTextBox, EmailPlaceholder);
            EmailTextBox.TextChanged += (s, e) => UpdatePlacholder(EmailTextBox, EmailPlaceholder);
        }

        private void UpdatePlacholder (TextBox box, TextBlock placeholder)
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
