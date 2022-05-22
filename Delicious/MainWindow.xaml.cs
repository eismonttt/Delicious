using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Users CurrentUser { get; set; }
        public List<RestaurantCard> Restaurants { get; set; } = new List<RestaurantCard>();
        public MainWindow(Users user)
        {
            InitializeComponent();
            CurrentUser = user;
            frame.Content = new RestaurantsPage(this);  // по умолчанию открывается страница с ресторанами
        }
        private void OpenRestaurants(object sender, RoutedEventArgs e)
        {
            frame.Content = new RestaurantsPage(this);
        }
        private void OpenDishes(object sender, RoutedEventArgs e)
        {
            frame.Content = new DishesPage(this);
        }
        private void OpenUserPage(object sender, RoutedEventArgs e)
        {
            frame.Content = new UserPage(this);
        }
    }
}
