using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BookPlaces)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FreePlaces)));
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

            }

        }

        private class UserViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private readonly User user;


            public string Username 
            {
                get => user.Username;
                set
                {
                    user.Username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
                }
            }
            public string Password 
            {
                get => user.Password;
                set
                {
                    user.Password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
            public string Name 
            {
                get => user.Name;
                set
                {
                    user.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }

            public bool IsAdmin 
            {
                get => user.IsAdmin;
                set
                {
                    user.IsAdmin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin)));
                }
            }


            public UserViewModel(User user)
            {
                this.user = user;
            }

            public UserViewModel() : this(new User())
            {

            }
        }

        private class AdminPageViewModel : ICommand
        {

            private readonly DeliciousEntities deliciousEntities;
            private readonly ObservableCollection<RestourauntsViewModel> restaurants;
            private ObservableCollection<UserViewModel> users;
            private readonly List<RestourauntsViewModel> originRestaraunts;

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public AdminPageViewModel()
            {
                deliciousEntities = new DeliciousEntities();
                restaurants = new ObservableCollection<RestourauntsViewModel>();
                originRestaraunts = new List<RestourauntsViewModel>();
            }

            public IEnumerable GetRestorauntSource()
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

                return restaurants;
            }

            public IEnumerable GetUserSource()
            {
                var usersFromDb = deliciousEntities.Users
                    .ToArray()
                    .Select(x => new UserViewModel(x));

                users = new ObservableCollection<UserViewModel>(usersFromDb);

                return users;
            }

            public bool CanExecute(object parameter)
            {
                return !restaurants.Any(x => x.FreePlaces < 0);
            }

            public void Execute(object parameter)
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
            }
        }

        private readonly AdminPageViewModel dataContext;

        public Page1Admin()
        {
            InitializeComponent();
            dataContext = new AdminPageViewModel();
            DataContext = dataContext;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            restGrid.ItemsSource = dataContext.GetRestorauntSource();
            userGrid.ItemsSource = dataContext.GetUserSource();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
