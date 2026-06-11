using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Placanje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be greater than 0!")]
        [DisplayName("Amount: ")]
        public double Iznos { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Sesija")]
        public int SesijaId { get; set; }
        public Sesija Sesija { get; set; }

        [ForeignKey("Rezervacija")]
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }

        [Required]
        public string MetodaPlacanja { get; set; }

        public DateTime DatumPlacanja { get; set; }
    }
}
