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
    /// Логика взаимодействия для DishesPage.xaml
    /// </summary>
    public partial class DishesPage : Page
    {
        public List<DishCard> PageDishes { get; set; } = new List<DishCard>();

        public MainWindow ParentWindow { get; set; }
        public DishesPage(MainWindow w)
        {
            InitializeComponent();

            ParentWindow = w;

            using (DeliciousEntities context = new DeliciousEntities())
            {
                List<Dishes> dishes = context.Dishes.ToList();
                foreach (Dishes dish in dishes)
                {
                    PageDishes.Add(new DishCard(dish, ParentWindow));
                }
            }

            dishesContainer.ItemsSource = PageDishes;
        }
    }
}
