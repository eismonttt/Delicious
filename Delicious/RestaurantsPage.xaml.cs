using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для Restaurants.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        public List<RestaurantCard> PageRestaurants { get; set; } = new List<RestaurantCard>();
        public MainWindow ParentWindow { get; set; }
        public bool IsAdmin => ParentWindow.CurrentUser.IsAdmin;
        public RestaurantsPage(MainWindow w)
        {
            InitializeComponent();
            ParentWindow = w;     // сохраняем главное окно
            ClickToAdm.Visibility = IsAdmin ? Visibility.Visible : Visibility.Hidden;

            using (DeliciousEntities context = new DeliciousEntities())  // подключаемся к БД
            {
                List<Restaurants> restaurants = context.Restaurants.ToList(); // берем все рестораны
                foreach (Restaurants restaurant in restaurants)
                {
                    PageRestaurants.Add(new RestaurantCard(restaurant, ParentWindow));
                }
            }
            restaurantsContainer.ItemsSource = PageRestaurants;
        }
        private void ClickToAdm_Click(object sender, RoutedEventArgs e)
        {
            Page1Admin adm = new Page1Admin();
            adm.Show();
        }
    }
}
