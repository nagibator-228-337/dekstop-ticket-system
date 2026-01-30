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

namespace DTS.MainPages
{
    /// <summary>
    /// Логика взаимодействия для TicketView.xaml
    /// </summary>
    public partial class TicketView : Window
    {
        public TicketView(Ticket ticket)
        {
            InitializeComponent();

            SubjectTextBox.Text = ticket.Subject;
            DescriptionTextBox.Text = ticket.Description;
            this.Title = $"Ticket: {ticket.AccessCode}";
        }
    }
}
