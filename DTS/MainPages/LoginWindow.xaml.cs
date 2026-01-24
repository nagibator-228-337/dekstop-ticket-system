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

namespace DTS
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {   

        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_click (object sender, RoutedEventArgs e)
        {
            var db = new DTS.Data.DataBase();
            if (db.ValidateLogin(LoginTextBox.Text.Trim(), PasswordTextBox.Password, out string fullName))
            {
                if (Application.Current.MainWindow is MainWindow main)
                {
                    main.MainFrame.Navigate(new MainEmployeePage());
                }
                else
                {
                    var window = new MainWindow();
                    window.MainFrame.Navigate(new MainEmployeePage());
                    window.Show();
                }

                this.Close();
            }

            else
            {
                MessageBox.Show("Wron login or pass");
            }
        }
    }

    
}
