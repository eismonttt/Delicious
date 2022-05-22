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
    /// Логика взаимодействия для DishCard.xaml
    /// </summary>
    public partial class DishCard : UserControl
    {
        public MainWindow ParentWindow { get; set; }
        public Dishes CardDish { get; set; }

        public DishCard(Dishes dish, MainWindow w)
        {
            InitializeComponent();

            ParentWindow = w;

            CardDish = dish;

            title.Text = CardDish.Name;
            descr.Text = CardDish.Description;
            weight.Text = (Math.Round((double)CardDish.Weight, 2)).ToString();
            rating.Text = (Math.Round((double)CardDish.Rating, 2)).ToString();

            string imageWay = "../Images/Dishes/" + CardDish.Image.Trim() + ".jpg";
            image.Source = new BitmapImage(new Uri(imageWay, UriKind.Relative));
        }

        private void OpenDishPage(object sender, RoutedEventArgs e)
        {
            ParentWindow.frame.Content = new DishPage(CardDish, ParentWindow);
        }
    }
}
