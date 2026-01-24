using DTS.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public MainEmployeePage()
        {
            InitializeComponent();

            DataBase db = new DataBase();
            _tickets = db.GetAllTickets();

            TicketsGrid.ItemsSource = _tickets;
        }
    }
}
