using DTS.Data;
using System.Windows;
using System.Diagnostics;

namespace DTS.MainPages
{
    public partial class TicketView : Window
    {
        private readonly Ticket _ticket;

        public TicketView(Ticket ticket, bool isEmployee)
        {
            InitializeComponent();
            _ticket = ticket;

            DataContext = _ticket;

            var db = new DataBase();
            EmployeeComboBox.ItemsSource = db.GetAllEmployees();
            EmployeeComboBox.DisplayMemberPath = "FullName";

            Title = $"Ticket: {ticket.AccessCode}";

            if (isEmployee)
            {
                ForClient.Visibility = Visibility.Collapsed;
                ForEmployee.Visibility = Visibility.Visible;
                ChatHeader.Text = "Chat with Client";
            }
            else
            {
                ForClient.Visibility = Visibility.Visible;
                ForEmployee.Visibility = Visibility.Collapsed;
                ChatHeader.Text = "Chat with Support";
            }
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is Employee selectedEmployee)
            {
                Debug.WriteLine($"PinClick BEFORE: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}, selectedEmployeeId={selectedEmployee.Id}");

                _ticket.AssignedEmployee = selectedEmployee; 
                new DataBase().UpdateAssignedEmployee(_ticket, selectedEmployee.Id); 

                Debug.WriteLine($"PinClick AFTER: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}");
                MessageBox.Show($"Employee '{selectedEmployee.FullName}' pinned to ticket!");
            }
            else
            {
                Debug.WriteLine("PinClick: no employee selected");
                MessageBox.Show("No employee selected!");
            }
        }
    }
}
