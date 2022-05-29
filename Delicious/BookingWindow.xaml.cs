using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        public MainWindow ParentWindow { get; set; }
        public RestaurantsPlaces Place { get; set; }
        public RestaurantPage RestaurantPage { get; set; }

        public BookingWindow(RestaurantsPlaces place, MainWindow w, RestaurantPage restPage)
        {
            InitializeComponent();

            ParentWindow = w;
            Place = place;
            RestaurantPage = restPage;

            prepurchase.Text = (Place.PlacePrice / 2).ToString();
        }

        private void BookClick(object sender, RoutedEventArgs e)
        {
            // записываем выбранную дату из календарика
            DateTime? selectedDate = calendar.Value;

            if (selectedDate != null)
            { 
                // дата на которую забронирован столик в нужном формате (без минут)
                string bookDateTime = selectedDate.Value.ToString("dd-MM-yyyy HH:00", CultureInfo.CurrentCulture);

                // объект нового бронирования места
                Orders newOrder = new Orders()
                {
                    UserId = ParentWindow.CurrentUser.Id,
                    RestaurantPlaceId = Place.Id,
                    BookDate = bookDateTime
                };

                using (DeliciousEntities context = new DeliciousEntities())
                {
                    // добавляем в бд
                    context.Orders.Add(newOrder);

                    // уменьшаем кол-во свободных столиков конкретного типа в этом ресторане 
                    context.RestaurantsPlaces.Where(place => place.Id == Place.Id).FirstOrDefault().CurrentPlaceCount = Place.CurrentPlaceCount - 1;

                    context.SaveChanges();
                }

                List<BookingControl> restNewBookings = new List<BookingControl>();
                using (DeliciousEntities context = new DeliciousEntities())
                {
                    // формируем новый список мест конкретно этого ресторана
                    List<RestaurantsPlaces> restPlaces = context.RestaurantsPlaces
                        .Include("Places")
                        .Where(places => places.RestaurantId == RestaurantPage.Restaurant.Id).ToList();

                    // заполняем список брони элементами управления
                    foreach (RestaurantsPlaces place in restPlaces)
                    {
                        restNewBookings.Add(new BookingControl(place, ParentWindow, RestaurantPage));
                    }
                }
                // Выводим новый список
                RestaurantPage.placesContainer.ItemsSource = restNewBookings;

                MessageBox.Show("Бронь прошла успешно!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите дату.");
                return;
            }
        }
        
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
