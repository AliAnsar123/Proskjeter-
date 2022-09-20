using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppeoppgave_1.Models
{
    public class ZipCode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Required(ErrorMessage = "Zip code is required"), RegularExpression(@"[0-9]{4}", ErrorMessage = "Zip code must be a four digit number with trailing zeros")]
        public string Id { get; set; }
        [Required(ErrorMessage = "City is required"), RegularExpression(@"^[a-zA-ZæøåÆØÅ .'-]{2,20}", ErrorMessage = "City can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters")]
        public string City { get; set; }
    }
}