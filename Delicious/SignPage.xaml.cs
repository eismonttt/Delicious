using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для SignPage.xaml
    /// </summary>
    public partial class SignPage : Page
    {
        // храним окно регистрации/входа
        public LoginWindow LoginWindow { get; set; }

        private Regex usernameRegex = new Regex(@"^[A-Za-z]\w{3,20}$");
        private Regex passwordRegex = new Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,20}$");
        private Regex userRealNameRegex = new Regex(@"^[A-Za-zА-Яа-я][a-zа-я]{0,20}$");

        public SignPage(LoginWindow w)
        {
            InitializeComponent();
            LoginWindow = w;
        }

        private void SignUser(object sender, RoutedEventArgs e)
        {
            string _username = username.Text;
            string _password = password.Password;

            username.BorderBrush = (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFCDEACE");
            password.BorderBrush = (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFCDEACE");

            bool usernameMatch = usernameRegex.IsMatch(_username);
            bool passwordMatch = passwordRegex.IsMatch(_password);

            if (!usernameMatch)
            {
                username.BorderBrush = Brushes.Red;
            }
            if (!passwordMatch)
            {
                password.BorderBrush = Brushes.Red;
            }

            // если введенные данные валидны
            if (usernameMatch && passwordMatch)
            {
                Users user;
                using (DeliciousEntities context = new DeliciousEntities())
                {
                    user = context.Users.Where(u => u.Username == _username && u.Password == _password).FirstOrDefault();
                }
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.");
                }
                else
                {
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    LoginWindow.Close();
                }
            }

        }
        private void BackPage(object sender, RoutedEventArgs e)
        {
            LoginWindow.loginFrame.GoBack();
        }
    }
}
