using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum StatusRezervacije
    {
        [Display(Name = "Active")] Aktivna, 
        [Display(Name = "Canceled")] Otkazana, 
        [Display(Name = "Expired")] Istekla, 
        [Display(Name = "Completed")] Zavrsena
    }
}
