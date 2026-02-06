using DTS.Data;
using System.Windows;
using System.Diagnostics;
using System.Linq;

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
            var employees = db.GetAllEmployees();
            employees.Insert(0, new Employee { Id = -1, FullName = "None" });

            EmployeeComboBox.ItemsSource = employees;
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

            // select current assigned employee in combobox (or "None" if null)
            if (_ticket.AssignedEmployee == null)
            {
                EmployeeComboBox.SelectedIndex = 0; // "None"
            }
            else
            {
                // Find the employee instance from the current items by Id.
                // This ensures SelectedItem matches by reference to an item from ItemsSource.
                var match = employees.FirstOrDefault(e => e.Id == _ticket.AssignedEmployee.Id);
                if (match != null)
                    EmployeeComboBox.SelectedItem = match;
                else
                    EmployeeComboBox.SelectedIndex = 0; // fallback to "None"
            }
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedItem is Employee selectedEmployee)
            {
                Debug.WriteLine($"PinClick BEFORE: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}, selectedEmployeeId={selectedEmployee.Id}");

                var db = new DataBase();

                if (selectedEmployee.Id == -1)
                {
                    db.UpdateAssignedEmployee(_ticket, (int?)null);
                    Debug.WriteLine($"PinClick AFTER UNASSIGN: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}");
                    MessageBox.Show("Назначение снято.");
                }
                else
                {
                    db.UpdateAssignedEmployee(_ticket, selectedEmployee.Id);
                    Debug.WriteLine($"PinClick AFTER: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}");
                    MessageBox.Show($"Сотрудник '{selectedEmployee.FullName}' закреплён за тикетом.");
                }
            }
            else
            {
                Debug.WriteLine("PinClick: no employee selected");
                MessageBox.Show("Сотрудник не выбран!");
            }
        }
    }
}
