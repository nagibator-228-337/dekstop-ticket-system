using DTS.Data;
using DTS.MainPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTS
{
    public partial class MainEmployeePage : Page
    {
        private ObservableCollection<Ticket> _tickets;
        private readonly string _fullName;
        private readonly bool _isAdmin;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private bool _isEmployee = true;
        

        public MainEmployeePage(string fullName, bool isAdmin, int id)
        {
            InitializeComponent();
            this.Loaded += (_, __) => { this.Focus(); Keyboard.Focus(this); };

            DataBase db = new DataBase();
            _tickets = db.GetAllTickets();
            _isAdmin = isAdmin;
            _fullName = fullName;

            AllTicketsGrid.ItemsSource = _tickets;

        }

        private void Ticket_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AllTicketsGrid.SelectedItem is Ticket ticket)
            {
                var window = new TicketView(ticket, _isEmployee);
                window.ShowDialog();
            }
        }

        private void ApplyRole()
        {
            if (_isAdmin)
            {
                //here need be part with code for creating window, where admin can create new profiles
            }
        }
    }
}
