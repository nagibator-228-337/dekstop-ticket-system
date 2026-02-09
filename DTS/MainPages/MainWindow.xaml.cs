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

    public partial class MainWindow : Window
    {
        public static CreateTicketWindow createTicketWindow;
        public static FindTicketWindow findTicketWindow;
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            var topBar = new TopBar();
            TopGrid.Children.Insert(0, topBar);
        }


        private void LoginButton_click( object sender, RoutedEventArgs e)
        {
            var window = new LoginWindow();
            Drakening.Visibility = Visibility.Visible;
            // ensure darkening hides when login window is closed
            window.Closed += (s, args) => Drakening.Visibility = Visibility.Collapsed;
            window.ShowDialog();
            // in case Closed handler didn't run for some reason, hide after dialog returns
            Drakening.Visibility = Visibility.Collapsed;
        }

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {       
            if (createTicketWindow == null)
            {
                createTicketWindow = new CreateTicketWindow();
                createTicketWindow.Closed += (s, args) => { createTicketWindow = null; Drakening.Visibility = Visibility.Collapsed; };
                Drakening.Visibility = Visibility.Visible;
                createTicketWindow.ShowDialog();
                Drakening.Visibility = Visibility.Collapsed;
            }
            else
            {
                createTicketWindow.Activate();
            }
        }

        private void SearchButton_click(object sender, RoutedEventArgs e)
        {
            if (findTicketWindow == null)
            {
                findTicketWindow = new FindTicketWindow();
                findTicketWindow.Closed += (s, args) => { findTicketWindow = null; Drakening.Visibility = Visibility.Collapsed; };
                Drakening.Visibility = Visibility.Visible;
                findTicketWindow.ShowDialog();
                Drakening.Visibility = Visibility.Collapsed;
            }
            else
            {
                findTicketWindow.Activate();
            }
        }

    }
}