using System.Collections.Generic;
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
            private int capacity;

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

            public int Capacity
            {
                get => capacity;
                set
                {
                    capacity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Capacity)));
                }
            }

            public RestourauntsViewModel(Restaurants restaurants)
            {
                Restaurants = restaurants;

            }

        }


        public DeliciousEntities deliciousEntities;
        private readonly ObservableCollection<RestourauntsViewModel> restaurants;
        private readonly List<RestourauntsViewModel> originRestaraunts;
        public Page1Admin()
        {
            InitializeComponent();
            deliciousEntities = new DeliciousEntities();
            restaurants = new ObservableCollection<RestourauntsViewModel>();
            originRestaraunts = new List<RestourauntsViewModel>();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            deliciousEntities
                .Restaurants
                .Include(x => x.RestaurantsPlaces)
                .ToArray()
                .Select(x => new RestourauntsViewModel(x))
                .ToList()
                .ForEach(x =>
                {
                    restaurants.Add(x);
                    originRestaraunts.Add(x);
                });

            restGrid.ItemsSource= restaurants;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var changedRestaraunts = restaurants
                .Intersect(originRestaraunts)
                .ToArray();

            var newRestaraunts = restaurants
                .Except(originRestaraunts)
                .ToArray();

            foreach (var item in newRestaraunts)
            {
                Places places = new Places()
                {
                    PlaceCapacity = item.Capacity
                };

                item.Restaurants.RestaurantsPlaces = new List<RestaurantsPlaces>()
                {
                    new RestaurantsPlaces()
                    {
                        RestaurantId = item.Id, 
                        Places = places
                    }
                };
            }

            deliciousEntities.Restaurants.BulkUpdate(changedRestaraunts.Select(x => x.Restaurants));
            deliciousEntities.Restaurants.AddRange(newRestaraunts.Select(x => x.Restaurants));
            deliciousEntities.SaveChanges();

            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
