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

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
