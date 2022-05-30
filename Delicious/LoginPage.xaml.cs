using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginWindow LoginWindow { get; set; }
        // создаем патерны 
        private Regex usernameRegex = new Regex(@"^[A-Za-z]\w{3,20}$");
        private Regex passwordRegex = new Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,20}$");
        private Regex userRealNameRegex = new Regex(@"^[A-Za-zА-Яа-я][a-zа-я]{0,20}$");

        public LoginPage(LoginWindow w)
        {
            InitializeComponent();
            LoginWindow = w;
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string _username = username.Text;   // создаем переменные и закидываем значение, введенное с поля
            string _password = password.Password;
            string _userRealName = userRealName.Text;
            
            // задаем цвет границы полей ввода как обычный
            username.BorderBrush = (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFCDEACE");//устанавливаем ободку новый цвет. поле.обводка создаем новый объект и придаем значение цвета
            password.BorderBrush = (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFCDEACE");
            userRealName.BorderBrush = (SolidColorBrush)(new BrushConverter()).ConvertFrom("#FFCDEACE");

            bool usernameMatch = usernameRegex.IsMatch(_username); // проверяем на соответствие введенной строчки и патерна
            bool passwordMatch = passwordRegex.IsMatch(_password);
            bool userRealNameMatch = userRealNameRegex.IsMatch(_userRealName);

            // если не совпало, то поля становятся красным 
            if (!usernameMatch)
            {
                username.BorderBrush = Brushes.Red;
            }
            if (!passwordMatch)
            {
                password.BorderBrush = Brushes.Red;
            }
            if (!userRealNameMatch)
            {
                userRealName.BorderBrush = Brushes.Red;
            }

            // если все введено правильно
            if (usernameMatch && passwordMatch && userRealNameMatch)
            {
                List<User> alreadyUsers = new List<User>();

                // создаем обращение к бд
                using(DeliciousEntities context = new DeliciousEntities())
                {
                    alreadyUsers = context.Users.Where(user => user.Username == _username && user.Password == _password).ToList();
                }

                // пользователь уже есть
                if(alreadyUsers.Count != 0)
                {
                    MessageBox.Show("Пользователь с таким логином и паролем уже есть.");
                }

                // если пользователя нет в бд, то регистрируем его 
                else
                {
                    User newUser = new User()
                    {
                        Username = _username,
                        Password = _password,
                        Name = _userRealName
                    };
                    // создаем объект главного окна
                    using (DeliciousEntities context = new DeliciousEntities())
                        
                    {
                        context.Users.Add(newUser);
                        context.SaveChanges(); /*сохранить изменения*/
                    }
                    
                    MainWindow mainWindow = new MainWindow(newUser);
                    mainWindow.Show();
                    LoginWindow.Close();
                }
            }
        }

        private void BackPage(object sender, RoutedEventArgs e)
        {
            // используем готовый метод, чтобы вернуться назад по истории страниц во фрейме
            LoginWindow.loginFrame.GoBack();
        }
    }
}
