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
    /// Логика взаимодействия для BookingControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public MainWindow ParentWindow { get; set; }
        public UserPage UserPage { get; set; }
        public Orders Order { get; set; }

        public OrderControl(Orders order, MainWindow w, UserPage userPage)
        {
            InitializeComponent();

            ParentWindow = w;
            UserPage = userPage;
            Order = order;

            places.Text = Order.RestaurantsPlaces.Places.PlaceCapacity.ToString();
            restaurant.Text = Order.RestaurantsPlaces.Restaurants.Name;
            orderDate.Text = Order.BookDate;
        }

        private void CancelBook(object sender, RoutedEventArgs e)
        {
            // получаем ресторан этого заказа
            Restaurants orederRestaurant = Order.RestaurantsPlaces.Restaurants;

            using (DeliciousEntities context = new DeliciousEntities())
            {
                // удаляем заказ
                // если объекта не существует локально
                if (!context.Orders.Local.Contains(Order))
                {
                    // тогда взять его копию локально
                    context.Orders.Attach(Order);
                }
                // удалить объект
                context.Orders.Remove(Order);

                // увеличиваем кол-во свободных мест в ресторане 
                context.RestaurantsPlaces.Where(place => place.Id == Order.RestaurantPlaceId).FirstOrDefault().CurrentPlaceCount += 1;
                context.SaveChanges();
            }

            List<OrderControl> NewOrders = new List<OrderControl>();
            using (DeliciousEntities context = new DeliciousEntities())
            {
                // формируем новый список мест конкретно этого ресторана
                List<Orders> newOrders = context.Orders
                    .Where(order => order.UserId == ParentWindow.CurrentUser.Id)
                    .AsDbQuery()
                    .Include(x=>x.RestaurantsPlaces)
                    .AlsoInclude(x=>x.Places)
                    .AlsoInclude(x=>x.Restaurants)
                    .ToList();

                // заполняем список брони элементами управления
                foreach (Orders order in newOrders)
                {
                    NewOrders.Add(new OrderControl(order, ParentWindow, UserPage));
                }
            }
            // Выводим новый список
            UserPage.ordersContainer.ItemsSource = NewOrders;
        }
    }
}
