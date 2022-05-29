using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Delicious 

//Набор методов, которые определяются сопоставление между классами и их свойствами
//и таблицами и их столбцами

{
    public class DeliciousEntities : DbContext /* DbContext: определяет контекст данных, используемый для взаимодействия с базой данных*/

    {
        public DeliciousEntities()
            : base("name=DbConnection")
        {

        }

        public DbSet<Dishes> Dishes { get; set; }  // DbSet<TEntity>: представляет набор сущностей, хранящихся в бд
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Places> Places { get; set; }
        public DbSet<Restaurants> Restaurants { get; set; }
        public DbSet<RestaurantsPlaces> RestaurantsPlaces { get; set; }
        public DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        //DbModelBuilder: сопоставляет классы на языке C# с сущностями в базе данных.

            modelBuilder.Entity<Dishes>()
                .HasMany(x => x.Menus)
                .WithRequired(x => x.Dishes)
                .HasForeignKey(x => x.DishId);

            modelBuilder.Entity<Restaurants>()
                .HasMany(x => x.Menus)
                .WithRequired(x => x.Restaurants)
                .HasForeignKey(x => x.RestaurantId);

            modelBuilder.Entity<Restaurants>()
                .HasMany(x => x.RestaurantsPlaces)
                .WithRequired(x => x.Restaurants)
                .HasForeignKey(x => x.RestaurantId);

            modelBuilder.Entity<RestaurantsPlaces>()
                .HasMany(x => x.Orders)
                .WithRequired(x => x.RestaurantsPlaces)
                .HasForeignKey(x => x.RestaurantPlaceId);

            modelBuilder.Entity<Places>()
                .HasMany(x => x.RestaurantsPlaces)
                .WithRequired(x => x.Places)
                .HasForeignKey(x => x.PlaceId);

            modelBuilder.Entity<Users>()
                .HasMany(x => x.Orders)
                .WithRequired(x => x.Users)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Menus>()
                .HasRequired(x => x.Restaurants)
                .WithMany(x => x.Menus)
                .HasForeignKey(x => x.RestaurantId);

            modelBuilder.Entity<Menus>()
                .HasRequired(x => x.Dishes)
                .WithMany(x => x.Menus)
                .HasForeignKey(x => x.DishId);

            modelBuilder.Entity<Orders>()
                .HasRequired(x => x.RestaurantsPlaces)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.RestaurantPlaceId);

            modelBuilder.Entity<Orders>()
                .HasRequired(x => x.Users)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<RestaurantsPlaces>()
                .HasRequired(x => x.Restaurants)
                .WithMany(x => x.RestaurantsPlaces)
                .HasForeignKey(x => x.RestaurantId);

            modelBuilder.Entity<RestaurantsPlaces>()
                .HasRequired(x => x.Places)
                .WithMany(x => x.RestaurantsPlaces)
                .HasForeignKey(x => x.PlaceId);

            base.OnModelCreating(modelBuilder);
        }
    }

}
