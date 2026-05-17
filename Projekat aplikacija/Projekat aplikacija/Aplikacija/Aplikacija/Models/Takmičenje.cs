using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Takmicenje
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }
        public string Igra { get; set; }
        public DateTime Datum { get; set; }

        public int GetId() => Id;
    }
}
