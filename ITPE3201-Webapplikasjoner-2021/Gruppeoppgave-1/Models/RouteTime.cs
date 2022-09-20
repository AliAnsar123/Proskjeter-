using System;
using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class RouteTime
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Route is required")]
        public virtual Route Route { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Direction is required")]
        public int Direction { get; set; }
    }
}
