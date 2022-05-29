using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Логика взаимодействия для Page1Admin.xaml
    /// </summary>
    public partial class Page1Admin : Window
    {
        private readonly DeliciousEntities deliciousEntities;
        private ObservableCollection<Restaurants> restaurants;
        public Page1Admin()
        {
            InitializeComponent();
            deliciousEntities = new DeliciousEntities();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var usersFromDB = deliciousEntities.Restaurants.ToArray();
            restaurants = new ObservableCollection<Restaurants>(usersFromDB);
            restGrid.ItemsSource= restaurants;
            restaurants.CollectionChanged += OnChanged;

        }

        private void OnChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var items = e.NewItems as IEnumerable<Restaurants>;
            if (items == null || items.Any(x => x.ClosesTime == null || x.Image == null || x.Name==null || x.Location==null || x.OpensTime==null))
            {
                return;
            }
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    deliciousEntities.Restaurants.AddRange(items);
                    deliciousEntities.SaveChanges();
                    break;
            }
        }
    }
}
