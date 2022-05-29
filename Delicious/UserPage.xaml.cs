using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Z.EntityFramework.Extensions;

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
                List<Orders> orders = context.Orders
                    .AsDbQuery()
                    .Include(x=>x.RestaurantsPlaces)
                    .AlsoInclude(x=>x.Places)
                    .AlsoInclude(x=>x.Restaurants)
                    .Include(nameof(Orders.Users))

                    .Where(order => order.UserId == User.Id)
                    
                    .ToList();
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
