using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Takmicenje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name: ")]
        public string Naziv { get; set; }

        [Required]
        [DisplayName("Game: ")]
        public string Igra { get; set; }

        [Required]
        [DisplayName("Date: ")]
        public DateTime Datum { get; set; }

        public int GetId() => Id;
    }
}
