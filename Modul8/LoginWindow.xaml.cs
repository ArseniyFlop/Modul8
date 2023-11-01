using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modul8
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        public bool IsAuthenticated { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = loginTextBox.Text;
            string password = passwordBox.Password;

            // Проверка логина и пароля
            if (username == "admin" && password == "admin")
            {
                IsAuthenticated = true;
                // Вход выполнен успешно, закрываем окно входа
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
        }
    }
}
