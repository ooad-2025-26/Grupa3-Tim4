using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum StatusKvara
    {
        [Display(Name = "Reported")]
        Prijavljen,
        [Display(Name = "In Progress")]
        UObradi,
        [Display(Name = "Resolved")]
        Otklonjen
    }
}
