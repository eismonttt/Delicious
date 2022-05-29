using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Delicious
{
    /// <summary>
    /// Логика взаимодействия для Page1Admin.xaml
    /// </summary>
    public partial class Page1Admin : Window
    {
        private class RestourauntsViewModel : INotifyPropertyChanged
        {
            public Restaurants Restaurants { get; }

            public event PropertyChangedEventHandler PropertyChanged;

            public int Id { get; set; }
            public string Name
            {
                get => Restaurants.Name;
                set
                {
                    Restaurants.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
            public string Location
            {
                get => Restaurants.Location;
                set
                {
                    Restaurants.Location = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
                }
            }
            public string Image
            {
                get => Restaurants.Image;
                set
                {
                    Restaurants.Image = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
            public int? OpensTime
            {
                get => Restaurants.OpensTime;
                set
                {
                    Restaurants.OpensTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpensTime)));
                }
            }
            public int? ClosesTime
            {
                get => Restaurants.ClosesTime;
                set
                {
                    Restaurants.ClosesTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClosesTime)));
                }
            }

            public RestourauntsViewModel(Restaurants restaurants)
            {
                Restaurants = restaurants;

            }
            public RestourauntsViewModel()
            {
                Restaurants = new Restaurants();
            }
        }


        public DeliciousEntities deliciousEntities;
        private ObservableCollection<RestourauntsViewModel> restaurants;
        public Page1Admin()
        {
            InitializeComponent();
            deliciousEntities = new DeliciousEntities();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var usersFromDB = deliciousEntities
                .Restaurants
                .ToArray()
                .Select(x => new RestourauntsViewModel(x));


            restaurants = new ObservableCollection<RestourauntsViewModel>(usersFromDB);
            restGrid.ItemsSource= restaurants;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var changedRestaraunts = restaurants.Select(x => x.Restaurants).ToArray();
            deliciousEntities.BulkUpdate(changedRestaraunts);
            deliciousEntities.SaveChanges();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
