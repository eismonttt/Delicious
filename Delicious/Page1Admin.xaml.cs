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

            public User User { get; }

            public string Username 
            {
                get => User.Username;
                set
                {
                    User.Username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
                }
            }
            public string Password 
            {
                get => User.Password;
                set
                {
                    User.Password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
            public string Name 
            {
                get => User.Name;
                set
                {
                    User.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }

            public bool IsAdmin 
            {
                get => User.IsAdmin;
                set
                {
                    User.IsAdmin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin)));
                }
            }


            public UserViewModel(User user)
            {
                this.User = user;
            }

            public UserViewModel() : this(new User())
            {

            }
        }

        private class AdminPageViewModel : ICommand
        {

            private readonly DeliciousEntities deliciousEntities;
            private readonly ObservableCollection<RestourauntsViewModel> restaurants;
            private readonly List<RestourauntsViewModel> originRestaraunts;
            private readonly ObservableCollection<UserViewModel> users;
            private readonly List<UserViewModel> originUsers;

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
                users = new ObservableCollection<UserViewModel>();
                originUsers = new List<UserViewModel>();
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
                deliciousEntities.Users
                    .ToArray()
                    .Select(x => new UserViewModel(x))
                    .ToList()
                    .ForEach(x =>
                    {
                        users.Add(x);
                        originUsers.Add(x);
                    });

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

                var changedUsers = users
                    .Intersect(originUsers)
                    .ToArray();

                var newUsers = users
                    .Except(originUsers)
                    .Select(x => x.User)
                    .ToArray();

                var deletedUsers = originUsers
                    .Except(changedUsers)
                    .Select(x => x.User)
                    .ToArray();

                deliciousEntities.Restaurants.BulkUpdate(changedRestaraunts.Select(x => x.Restaurants));
                deliciousEntities.Restaurants.AddRange(newRestaraunts);
                deliciousEntities.Restaurants.RemoveRange(deletedRestoraunts);

                deliciousEntities.Users.BulkUpdate(changedUsers.Select(x => x.User));
                deliciousEntities.Users.AddRange(newUsers);
                deliciousEntities.Users.RemoveRange(deletedUsers);

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
