using DTS.Data;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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

            var employees = db.GetAllEmployees();
            employees.Insert(0, new Employee { Id = -1, FullName = "None" });
            EmployeeComboBox.ItemsSource = employees;
            EmployeeComboBox.DisplayMemberPath = "FullName";

            EmployeeComboBox.SelectedItem =
                _ticket.AssignedEmployee == null
                ? employees[0]
                : employees.FirstOrDefault(e => e.Id == _ticket.AssignedEmployee.Id) ?? employees[0];

            var statuses = Enum.GetValues(typeof(Ticket.TicketStatus))
                               .Cast<Ticket.TicketStatus>()
                               .ToList();
            StatusComboBox.ItemsSource = statuses;
            StatusComboBox.SelectedItem = _ticket.Status;

            StatusComboBox.SelectionChanged += (s, e) =>
            {
                if (StatusComboBox.SelectedItem is Ticket.TicketStatus status &&
                    status != _ticket.Status)
                {
                    db.UpdateTicketStatus(_ticket, status);
                }
            };

            Title = $"Ticket: {_ticket.AccessCode}";

            if (_isEmployee)
            {
                ForEmployee.Visibility = Visibility.Visible;
                ForClient.Visibility = Visibility.Collapsed;
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
            if (EmployeeComboBox.SelectedItem is not Employee selected) return;

            var db = DataBase.Instance;

            if (selected.Id == -1)
                db.UpdateAssignedEmployee(_ticket, null);
            else
                db.UpdateAssignedEmployee(_ticket, selected.Id);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WritingMessageTextBox.Text)) return;

            var message = new Message
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

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

    }
}
