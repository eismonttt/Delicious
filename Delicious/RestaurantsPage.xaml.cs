using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для Restaurants.xaml
    /// </summary>
    public partial class RestaurantsPage : Page
    {
        public ObservableCollection<RestaurantCard> PageRestaurants { get; set; } = new ObservableCollection<RestaurantCard>();
        public MainWindow ParentWindow { get; set; }
        public bool IsAdmin => ParentWindow.CurrentUser.IsAdmin;
        public RestaurantsPage(MainWindow w)
        {
            InitializeComponent();
            ParentWindow = w;     // сохраняем главное окно
            ClickToAdm.Visibility = IsAdmin ? Visibility.Visible : Visibility.Hidden;
            RefreshPageRestaurants();

        }
        private void ClickToAdm_Click(object sender, RoutedEventArgs e)
        {
            Page1Admin adm = new Page1Admin();
            if (adm.ShowDialog() ?? false)
            {
                RefreshPageRestaurants();
            }
        }

        private void RefreshPageRestaurants()
        {
            PageRestaurants.Clear();
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
