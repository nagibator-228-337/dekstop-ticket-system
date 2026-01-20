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
        public MainWindow()
        {
            InitializeComponent();
            
            //var window = new CreateTicketWindow();
            //window.Show(); // или window.ShowDialog();

        }

        private void LoginButton_click( object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainEmployeePage());
        }
    }
}