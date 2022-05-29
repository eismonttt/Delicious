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
    /// Логика взаимодействия для DishPage.xaml
    /// </summary>
    public partial class DishPage : Page
    {
        public Dishes Dish { get; set; }
        public MainWindow ParentWindow { get; set; }

        public List<RestaurantCard> DishRestaurants { get; set; } = new List<RestaurantCard>();

        public DishPage(Dishes dish, MainWindow w)
        {
            InitializeComponent();

            Dish = dish;
            ParentWindow = w;

            title.Text = Dish.Name;
            description.Text = Dish.Description;
            weight.Text = (Math.Round((double)Dish.Weight, 2)).ToString();
            rating.Text = (Math.Round((double)Dish.Rating, 2)).ToString();
            calories.Text = (Math.Round((double)Dish.Calories, 2)).ToString();
            price.Text = (Math.Round((double)Dish.Price, 2)).ToString();

            string imageWay = "../../Images/Dishes/" + Dish.Image.Trim() + ".jpg";
            dishPageHeader.ImageSource = new BitmapImage(new Uri(imageWay, UriKind.Relative));

            using (DeliciousEntities context = new DeliciousEntities())
            {
                List<Restaurants> dishRestaurants = context.Menus.Where(menu => menu.DishId == Dish.Id)
                    .AsDbQuery()
                    .Include(x=>x.Restaurants)
                    .Select(menu => menu.Restaurants).ToList();

                foreach (Restaurants rest in dishRestaurants)
                {
                    DishRestaurants.Add(new RestaurantCard(rest, ParentWindow));
                }
            }

            restContainer.ItemsSource = DishRestaurants;
        }

        private void BackPage(object sender, RoutedEventArgs e)
        {
            ParentWindow.frame.GoBack();
        }
    }
}
