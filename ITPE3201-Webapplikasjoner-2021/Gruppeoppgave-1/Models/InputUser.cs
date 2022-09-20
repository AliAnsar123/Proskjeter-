using System.ComponentModel.DataAnnotations;

namespace Gruppeoppgave_1.Models
{
    public class InputUser
    {
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ.\-]{2,20}$")]
        public string Username { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$")]
        public string Password { get; set; }
    }
}
