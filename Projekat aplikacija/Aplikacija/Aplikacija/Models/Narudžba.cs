using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        public string Proizvod { get; set; }
        public double Cijena { get; set; }
        public string Status { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int GetId() => Id;
    }
}
