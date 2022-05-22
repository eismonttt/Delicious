using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Delicious
{
    public class DeliciousEntities : DbContext
    {
        public DeliciousEntities()
            : base("name=DbConnection")
        {
        }

        public virtual DbSet<Dishes> Dishes { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Places> Places { get; set; }
        public virtual DbSet<Restaurants> Restaurants { get; set; }
        public virtual DbSet<RestaurantsPlaces> RestaurantsPlaces { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
