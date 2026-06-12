using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TipNarudzbe Tip { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "Order must be between 2 and 100 characters long.")]
        public string? Proizvod { get; set; }

        [StringLength(300, MinimumLength = 2, ErrorMessage = "Message must be between 2 and 300 characters long.")]
        public string? Poruka { get; set; }

        public DateTime VrijemeSlanja { get; set; } = DateTime.Now;

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Sesija")]
        public int SesijaId { get; set; }
        public Sesija Sesija { get; set; }
    }
}
