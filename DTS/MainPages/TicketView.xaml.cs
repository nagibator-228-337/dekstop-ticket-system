using DTS.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace DTS.MainPages
{
    public partial class TicketView : Window
    {
        private readonly Ticket _ticket;
        private readonly ObservableCollection<Message> _messages;
        private readonly bool _isEmployee;
        public bool IsEmployee => _isEmployee;
        private readonly int _employeeId;
        public TicketView(Ticket ticket, bool isEmployee, int emplooyeeId)
        {
            InitializeComponent();
            _ticket = ticket;
            _isEmployee = isEmployee;
            _employeeId = emplooyeeId;

            DataContext = _ticket;

            var db = DataBase.Instance;

            _messages = db.GetMessagesByTicketId(_ticket.Id);
            MessageItemControl.ItemsSource = _messages;


            // --- Employee ComboBox ---
            var employees = db.GetAllEmployees();
            employees.Insert(0, new Employee { Id = -1, FullName = "None" });
            EmployeeComboBox.ItemsSource = employees;
            EmployeeComboBox.DisplayMemberPath = "FullName";
            
            if (_ticket.AssignedEmployee == null)
                EmployeeComboBox.SelectedIndex = 0;
            else
            {
                var match = employees.FirstOrDefault(e => e.Id == _ticket.AssignedEmployee.Id);
                EmployeeComboBox.SelectedItem = match ?? employees[0];
            }

            // --- Status ComboBox ---
            var statuses = Enum.GetValues(typeof(Ticket.TicketStatus))
                               .Cast<Ticket.TicketStatus>()
                               .ToList();
            StatusComboBox.ItemsSource = statuses;
            StatusComboBox.SelectedItem = _ticket.Status;


            // status changing
            StatusComboBox.SelectionChanged += (s, e) =>
            {
                if (StatusComboBox.SelectedItem is Ticket.TicketStatus selectedStatus &&
                    selectedStatus != _ticket.Status)
                {
                    db.UpdateTicketStatus(_ticket, selectedStatus);
                }
            };


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

                var db = DataBase.Instance;

                if (selectedEmployee.Id == -1)
                {
                    db.UpdateAssignedEmployee(_ticket, (int?)null);
                    Debug.WriteLine($"PinClick AFTER UNASSIGN: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}");
                    MessageBox.Show("Assignment removed.");
                }
                else
                {
                    db.UpdateAssignedEmployee(_ticket, selectedEmployee.Id);
                    Debug.WriteLine($"PinClick AFTER: ticket.Id={_ticket.Id}, ticket.AssignedEmployee?.Id={_ticket.AssignedEmployee?.Id}");
                    MessageBox.Show($"Employee '{selectedEmployee.FullName}' assigned to the ticket.");
                }
            }
            else
            {
                Debug.WriteLine("PinClick: no employee selected");
                MessageBox.Show("No employee selected!");
            }
        }

        private void StatusChanged()
        {
            var db = DataBase.Instance;
            db.UpdateTicketStatus(_ticket, _ticket.Status);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(WritingMessageTextBox.Text))
            {
                Message message = new Message
                {
                    TicketId = _ticket.Id,
                    AuthorType = _isEmployee ? AuthorType.Employee : AuthorType.Client,
                    AuthorId = _isEmployee ? _employeeId : 0,
                    SentAt = DateTime.Now,
                    Text = WritingMessageTextBox.Text
                };

                var db = DataBase.Instance;
                db.AddMessage(message);
                _messages.Add(message);
                WritingMessageTextBox.Clear();

            }
        }
    }
}
