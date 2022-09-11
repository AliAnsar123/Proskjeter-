using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required"), RegularExpression(@"^[a-zA-ZæøåÆØÅ .'-]{2,20}", ErrorMessage = "First name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required"), RegularExpression(@"^[a-zA-ZæøåÆØÅ .'-]{2,20}", ErrorMessage = "Last name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters")]
        public string LastName { get; set; }
        //[Required(ErrorMessage = "Email is required"), RegularExpression(@".+@.+\..+", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Phone is required"), RegularExpression(@"[0-9]{8}", ErrorMessage = "Phone must contain 8 numbers")]
        public int Phone { get; set; }
        //[Required(ErrorMessage = "Street is required"), RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,50}", ErrorMessage = "Street can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 50 characters")]
        public string Street { get; set; }
        //[Required(ErrorMessage = "Zip code is required")]
        public virtual ZipCode ZipCode { get; set; }

        // TODO sjekk om denne kan legges til igjen
        //public virtual ICollection<Order> Orders { get; set; }
    }
}