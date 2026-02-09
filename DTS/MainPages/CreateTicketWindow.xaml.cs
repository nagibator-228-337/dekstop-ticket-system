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
using System.Threading.Tasks;

namespace DTS
{
    public partial class CreateTicketWindow : Window
    {
        private bool _unfilledField = false;
        public CreateTicketWindow()
        {
            InitializeComponent();
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

            var db = DataBase.Instance;
            db.AddTicket(ticket);

            CodeGrid.Visibility = Visibility.Visible;

            SubjectTextBox.IsEnabled = false;
            DescriptionTextBox.IsEnabled = false;
            ContactTextBox.IsEnabled = false;
            CreateButton.IsEnabled = false;

            CodeTextBlock.Text = ticket.AccessCode;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseCodeOverlay_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure? If you lose this code, you won't be able to open it again.",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
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
