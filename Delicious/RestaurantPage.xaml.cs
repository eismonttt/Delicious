using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для RestaurantPage.xaml
    /// </summary>
    public partial class RestaurantPage : Page
    {
        public Restaurants Restaurant { get; set; }
        public MainWindow ParentWindow { get; set; }

        public ObservableCollection<DishCard> RestDishes { get; set; } = new ObservableCollection<DishCard>();
        public ObservableCollection<BookingControl> BookingControls { get; set; } = new ObservableCollection<BookingControl>();

        public RestaurantPage(Restaurants r, MainWindow w)
        {

            InitializeComponent();
            placesContainer.ItemsSource = BookingControls;
            dishesContainer.ItemsSource = RestDishes;


            Restaurant = r;
            ParentWindow = w;

            title.Text = Restaurant.Name;
            location.Text = Restaurant.Location;

            string opens;
            string closes;
            if (Restaurant.OpensTime < 10)
            {
                opens = "0" + Restaurant.OpensTime.ToString();
            }
            else
            {
                opens = Restaurant.OpensTime.ToString();
            }
            if (Restaurant.ClosesTime < 10)
            {
                closes = "0" + Restaurant.ClosesTime.ToString();
            }
            else
            {
                closes = Restaurant.ClosesTime.ToString();
            }
            opensTime.Text = opens + ":00";
            closesTime.Text = closes + ":00";

            string imageWay = "../../Images/Restaurants/" + Restaurant.Image.Trim() + ".jpg";
            restHeaderImage.ImageSource = new BitmapImage(new Uri(imageWay, UriKind.Relative));

            using (DeliciousEntities context = new DeliciousEntities())
            {
                // выбираем все блюда, которые есть в меню этого ресторана
                List<Dishes> restDishes = context.Menus.Where(menu => menu.RestaurantId == Restaurant.Id).Select(menu => menu.Dishes).ToList();

                foreach (Dishes dish in restDishes)
                {
                    RestDishes.Add(new DishCard(dish, ParentWindow));
                }
            }


            using (DeliciousEntities context = new DeliciousEntities())
            {
                List<RestaurantsPlaces> restPlaces = context.RestaurantsPlaces.Where(places => places.RestaurantId == Restaurant.Id).ToList();

                foreach (RestaurantsPlaces place in restPlaces)
                {
                    BookingControls.Add(new BookingControl(place, ParentWindow, this));
                }
            }
        }

        private void BackPage(object sender, RoutedEventArgs e)
        {
            ParentWindow.frame.GoBack();
        }
    }
}
