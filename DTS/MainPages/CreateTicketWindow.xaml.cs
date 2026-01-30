using DTS.Data;
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
        private bool _unfilledField = false;
        public CreateTicketWindow()
        {
            InitializeComponent();

            //placeholder logic
            SubjectTextBox.GotFocus += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);
            SubjectTextBox.LostFocus += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);
            SubjectTextBox.TextChanged += (s, e) => UpdatePlacholder(SubjectTextBox, SubjectPlaceHolder);

            DescriptionTextBox.GotFocus += (s, e) => UpdatePlacholder(DescriptionTextBox, DescriptionTextPlaceholder);
            DescriptionTextBox.LostFocus += (s, e) => UpdatePlacholder(DescriptionTextBox, DescriptionTextPlaceholder);
            DescriptionTextBox.TextChanged += (s, e) => UpdatePlacholder(DescriptionTextBox, DescriptionTextPlaceholder);

            ContactTextBox.GotFocus += (s, e) => UpdatePlacholder(ContactTextBox, ContactPlaceholder);
            ContactTextBox.LostFocus += (s, e) => UpdatePlacholder(ContactTextBox, ContactPlaceholder);
            ContactTextBox.TextChanged += (s, e) => UpdatePlacholder(ContactTextBox, ContactPlaceholder);
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

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SubjectTextBox.Text))
            {
                ErrorsHighlight(SubjectTextBox, SubjectUnfilledBlock);
                return;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                ErrorsHighlight(DescriptionTextBox, DescriptionUnfilledBlock);
                return;
            }
            

            Ticket ticket = new Ticket
            {
                Subject = SubjectTextBox.Text,
                Description = DescriptionTextBox.Text,
                CreatedAt = DateTime.Now,
                Status = Ticket.TicketStatus.New,
                AssignedEmployee = null,
                AccessCode = Guid.NewGuid().ToString(),
                ClientContact = ContactTextBox.Text
            };

            DataBase db = new DataBase();
            db.AddTicket(ticket);

            CodeGrid.Visibility = Visibility.Visible;

            SubjectTextBox.IsEnabled = false;
            DescriptionTextBox.IsEnabled = false;
            ContactTextBox.IsEnabled = false;
            CreateButton.IsEnabled = false;

            CodeTextBlock.Text = ticket.AccessCode;
        }

        private async void ErrorsHighlight(TextBox textBox, TextBlock textBlock)
        {
            var oldBrush = textBox.BorderBrush;
            var oldThickness = textBox.BorderThickness; //red borders

            textBlock.Visibility = Visibility.Visible; //error messaage

            textBox.BorderBrush = Brushes.Red;
            textBox.BorderThickness = new Thickness(2);

            await Task.Delay(3000);

            textBox.BorderBrush = oldBrush;
            textBox.BorderThickness = oldThickness;

            textBlock.Visibility = Visibility.Collapsed;
        }

        private void CopyCode_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CodeTextBlock.Text);
        }

    }
}
