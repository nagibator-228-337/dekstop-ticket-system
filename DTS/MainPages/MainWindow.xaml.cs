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
        public MainWindow()
        {
            InitializeComponent();
            
            //var window = new CreateTicketWindow();
            //window.Show(); // или window.ShowDialog();

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
                createTicketWindow.Show();
            }
            else
            {
                createTicketWindow.Activate(); //Prevents opening windows hundreds of times
            }
        }

        private void SearchButton_click(object sender, RoutedEventArgs e)
        {
            if (findTicketWindow == null)
            {
                findTicketWindow = new FindTicketWindow();
                findTicketWindow.Show();
            }
            else
            {
                findTicketWindow.Activate();
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}