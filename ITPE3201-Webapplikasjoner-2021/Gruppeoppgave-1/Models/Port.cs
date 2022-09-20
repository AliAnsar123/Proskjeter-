using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class Port
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required"), RegularExpression(@"^[0-9-a-zA-ZæøåÆØÅ .'-]{2,20}", ErrorMessage = "Name can only contain letters, spaces, punctation marks and hypens. And must be between 2 and 20 characters")]
        public string Name { get; set; }
    }
}
