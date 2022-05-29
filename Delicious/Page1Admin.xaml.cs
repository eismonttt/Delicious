using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            public RestaurantsPlaces RestaurantsPlaces { get; }
            public Places Places { get; }

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

            public bool IsNew { get; }

            public int Capacity
            {
                get => RestaurantsPlaces.PlaceCount;
                set
                {
                    var placesDiff = RestaurantsPlaces.PlaceCount - value;
                    RestaurantsPlaces.PlaceCount = value;
                    Places.PlaceCapacity = value;
                    RestaurantsPlaces.CurrentPlaceCount -= placesDiff;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Capacity)));
                }
            }

            public int BookPlaces => Capacity - RestaurantsPlaces.CurrentPlaceCount;
            public int FreePlaces => Capacity - BookPlaces;

            public RestourauntsViewModel(Restaurants restaurants)
            {
                Restaurants = restaurants;
                if (!Restaurants.RestaurantsPlaces.Any())
                {
                    Restaurants.RestaurantsPlaces.Add(new RestaurantsPlaces
                    {
                        Id = Restaurants.Id,
                        Places = new Places()
                    });
                }
                RestaurantsPlaces = Restaurants.RestaurantsPlaces.First();
                Places = RestaurantsPlaces.Places;
            }

            public RestourauntsViewModel() : this(new Restaurants())
            {
                IsNew |= true;
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
            restaurants.CollectionChanged += OnCollectionChanged;
            originRestaraunts = new List<RestourauntsViewModel>();
            Loaded += OnLoaded;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SaveChanges.IsEnabled = !restaurants.Any(x => x.FreePlaces < 0);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            deliciousEntities
                .Restaurants
                .Include(x => x.RestaurantsPlaces)
                .ThenInclude(x => x.Places)
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
                .Select(x => x.Restaurants)
                .ToArray();

            var deletedRestoraunts = originRestaraunts
                .Except(changedRestaraunts)
                .Select(x => x.Restaurants)
                .ToArray();

            deliciousEntities.Restaurants.BulkUpdate(changedRestaraunts.Select(x => x.Restaurants));
            deliciousEntities.Restaurants.AddRange(newRestaraunts);
            deliciousEntities.Restaurants.RemoveRange(deletedRestoraunts);
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
