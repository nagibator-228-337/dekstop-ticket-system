using DTS.Data;
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
    public partial class LoginWindow : Window
    {   

        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            var db = DataBase.Instance;

            if (db.ValidateLogin(
                LoginTextBox.Text.Trim(),
                PasswordTextBox.Password,
                out string fullName,
                out string role,
                out int id))
            {
                bool isAdmin = role == "Admin";
                var page = new MainEmployeePage(fullName, isAdmin, id);

                if (MainWindow.Instance != null)
                {
                    MainWindow.Instance.MainFrame.Navigate(page);
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong login or password");
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }


}
