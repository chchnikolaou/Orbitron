using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Orbitron.Models
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public Property Property { get; set; }

    }

    public enum Property
    {
        Client, 
        Seller, 
        Administrator
    }

}
