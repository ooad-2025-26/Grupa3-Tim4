using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum StatusSesije
    {
        [Display(Name = "Active")] Aktivna, 
        [Display(Name = "Completed")] Zavrsena, 
        [Display(Name = "Paused")] Pauzirana, 
        [Display(Name = "Expired")] Istekla
    }
}
