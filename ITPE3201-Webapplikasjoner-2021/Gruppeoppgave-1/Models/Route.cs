using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Origin port is required")]
        public virtual Port Origin { get; set; }
        [Required(ErrorMessage = "Destination port is required")]
        public virtual Port Destination { get; set; }
        [Required(ErrorMessage = "Company is required")]
        public virtual Company Company { get; set; }
    }
}
