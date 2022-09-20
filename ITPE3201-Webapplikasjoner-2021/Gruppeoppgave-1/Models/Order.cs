using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Departure route time is required")]
        public virtual RouteTime DepartureRouteTime { get; set; }
        public virtual RouteTime ReturnRouteTime { get; set; }
        [Required(ErrorMessage = "Vehicles is required"), RegularExpression(@"[0-9]{1}", ErrorMessage = "Vehicles must a be a number between 0 and 9")]
        public int NumberOfVehicles { get; set; }
        public bool IsRoundTrip { get; set; }

        [Required(ErrorMessage = "Main customer is required")]
        public virtual Customer MainCustomer { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
