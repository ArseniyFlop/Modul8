using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modul8
{
    public partial class MainWindow : Window
    {
        Data database = new Data();
        private bool isAuthenticated = false; // Флаг для проверки аутентификации

        public MainWindow()
        {
            InitializeComponent();

            // Открываем окно входа
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            // Проверяем результат входа
            isAuthenticated = loginWindow.IsAuthenticated;

            if (!isAuthenticated)
            {
                // Если пользователь не аутентифицирован, закрываем приложение
                Close();
            }
            else
            {
                // Если пользователь успешно вошел в систему, загружаем данные
                LoadPasswordEntries();
            }
        }


        private bool IsUserAuthenticated()
        {
            return true;
        }
        // Метод для загрузки данных из базы данных
        public void LoadPasswordEntries()
        {
            try
            {
                database.openConnection();

                string query = "SELECT * FROM Mod8_db";

                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    passwordListView.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                database.closeConnection();
            }
        }
        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database.openConnection();
                string query = "INSERT INTO [dbo].[Mod8_db] (Website, Username, Password) VALUES (@Website, @Username, @Password)";
                using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Website", websiteTextBox.Text);
                    command.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                    command.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                    command.ExecuteNonQuery();
                }
                LoadPasswordEntries();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private void DeletePassword_Click(object sender, RoutedEventArgs e)
        {
            if (passwordListView.SelectedItems.Count > 0)
            {
                DataRowView selectedRow = (DataRowView)passwordListView.SelectedItem;
                int transactionID = Convert.ToInt32(selectedRow["Id"]);

                try
                {
                    database.openConnection();

                    string query = "DELETE FROM Mod8_db WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, database.GetConnection()))
                    {
                        command.Parameters.AddWithValue("@Id", transactionID);
                        command.ExecuteNonQuery();
                    }

                    LoadPasswordEntries();
                    ResetForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
                finally
                {
                    database.closeConnection();
                }
            }
        }
        public class PasswordEntry
        {
            public string Website { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        private void ResetForm()
        {
            websiteTextBox.Text = "";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
        }
    }
}