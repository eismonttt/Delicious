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
            private string name;
            private string location;
            private string image;
            private int? opensTime;
            private int? closesTime;

            public event PropertyChangedEventHandler PropertyChanged;

            public int Id { get; set; }
            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
            public string Location
            {
                get => location;
                set
                {
                    location = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
                }
            }
            public string Image
            {
                get => image;
                set
                {
                    image = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
            public int? OpensTime
            {
                get => opensTime;
                set
                {
                    opensTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OpensTime)));
                }
            }
            public int? ClosesTime
            {
                get => closesTime;
                set
                {
                    closesTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClosesTime)));
                }
            }

            public RestourauntsViewModel(Restaurants restaurants)
            {
                Restaurants = restaurants;
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
                .Select(x => new RestourauntsViewModel(x))
                .ToArray();

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
