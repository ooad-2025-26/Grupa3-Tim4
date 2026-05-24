using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Aplikacija.Models
{
    public class Korisnik : IdentityUser
    {

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 30 characters long!")]
        [RegularExpression(@"^[a-zA-ZšđčćžŠĐČĆŽ ]+$", ErrorMessage = "Name can only contain letters and spaces!")]
        [DisplayName("Name: ")]
        public string Ime { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 30 characters long!")]
        [RegularExpression(@"^[a-zA-ZšđčćžŠĐČĆŽ ]+$", ErrorMessage = "Surname can only contain letters and spaces!")]
        [DisplayName("Surname: ")]
        public string Prezime { get; set; }
        
        [DisplayName("Membership Active: ")]
        public bool ClanarinaAktivna { get; set; }
    }
}
