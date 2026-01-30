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


        }


        private void LoginButton_click( object sender, RoutedEventArgs e)
        {
            var window = new LoginWindow();
            window.ShowDialog();
        }

        private void CreateButton_click(object sender, RoutedEventArgs e)
        {
            if (createTicketWindow == null)
            {
                createTicketWindow = new CreateTicketWindow();
                createTicketWindow.Closed += (s, args) => createTicketWindow = null;
                createTicketWindow.Show();
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
                findTicketWindow.Closed += (s, args) => findTicketWindow = null;
                findTicketWindow.Show();
            }
            else
            {
                findTicketWindow.Activate();
            }
        }

    }
}