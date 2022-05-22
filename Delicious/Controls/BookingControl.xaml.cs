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
    public partial class BookingControl : UserControl
    {
        public MainWindow ParentWindow { get; set; }
        public RestaurantsPlaces RestaurantPlace { get; set; }
        public RestaurantPage RestaurantPage { get; set; }

        public BookingControl(RestaurantsPlaces place, MainWindow w, RestaurantPage restPage)
        {
            InitializeComponent();

            ParentWindow = w;
            RestaurantPlace = place;
            RestaurantPage = restPage;

            // сколько мест на столик
            places.Text = RestaurantPlace.Places.PlaceCapacity.ToString();

            // сколько всего 
            placesAll.Text = RestaurantPlace.PlaceCount.ToString();

            // свободные столики
            int currentPlaces = RestaurantPlace.CurrentPlaceCount;
            placesCurrent.Text = currentPlaces.ToString();

            // если свободных столиков нет, то кнопка бронирования не активна
            if (currentPlaces == 0)
            {
                bookButton.IsEnabled = false;
            }
        }

        private void BookPlace(object sender, RoutedEventArgs e)
        {
            BookingWindow bookingWindow = new BookingWindow(RestaurantPlace, ParentWindow, RestaurantPage);
            bookingWindow.Show();
        }
    }
}
