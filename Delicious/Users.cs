
namespace Delicious
{
    using System.Collections.Generic;

    public sealed class User
    {
        public User()
        {
        
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public bool IsAdmin { get; set; }


        public ICollection<Orders> Orders { get; set; }
    }
}
