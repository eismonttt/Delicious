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
    /// Логика взаимодействия для RestaurantCard.xaml
    /// </summary>
    public partial class RestaurantCard : UserControl
    {
        public MainWindow ParentWindow { get; set; }
        public Restaurants Restaurant { get; set; }
        
        public RestaurantCard(Restaurants r, MainWindow w)
        {
            InitializeComponent();

            ParentWindow = w;
            Restaurant = r;

            title.Text = Restaurant.Name;
            location.Text = Restaurant.Location;

            string opens;
            string closes;
            if(Restaurant.OpensTime < 10)
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

            // формируем путь к картинке этого ресторана
            string imageWay = "../Images/Restaurants/" + Restaurant.Image.Trim() + ".jpg";

            // записываем путь к картинке используя переменную imageWay
            image.Source = new BitmapImage(new Uri(imageWay, UriKind.Relative));
        }
        private void OpenRestaurant(object sender, RoutedEventArgs e)
        {
            ParentWindow.frame.Content = new RestaurantPage(Restaurant, ParentWindow);
        }
    }
}
