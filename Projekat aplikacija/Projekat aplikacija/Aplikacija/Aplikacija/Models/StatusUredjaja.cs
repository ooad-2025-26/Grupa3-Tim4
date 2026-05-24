using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum StatusUredjaja
    {
        [Display(Name = "Available")] Slobodan, 
        [Display(Name = "In Use")] Zauzet, 
        [Display(Name = "Out of Order")] Neispravan, 
        [Display(Name = "Reserved")] Rezervisan
    }
}
