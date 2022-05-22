using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для Restaurants.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        public List<RestaurantCard> PageRestaurants { get; set; } = new List<RestaurantCard>();
        public MainWindow ParentWindow { get; set; }
        public RestaurantsPage(MainWindow w)
        {
            InitializeComponent();
            ParentWindow = w;     // сохраняем главное окно

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
    }
}
