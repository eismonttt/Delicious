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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public MainWindow ParentWindow { get; set; }
        public Users User { get; set; }
        public List<OrderControl> UserOrders { get; set; } = new List<OrderControl>();
        public UserPage(MainWindow w)
        {
            InitializeComponent();

            ParentWindow = w;
            User = ParentWindow.CurrentUser;

            userRealName.Text = User.Name;

            using (DeliciousEntities context = new DeliciousEntities())
            {
                List<Orders> orders = context.Orders.Where(order => order.UserId == User.Id).ToList();
                foreach (Orders order in orders)
                {
                    UserOrders.Add(new OrderControl(order, ParentWindow, this));
                }
            }
            ordersContainer.ItemsSource = UserOrders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            ParentWindow.Close();
        }
    }
}
