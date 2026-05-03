namespace Aplikacija.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        public DateTime VrijemeOd { get; set; }
        public DateTime VrijemeDo { get; set; }
        public StatusRezervacije Status { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int GetId() => Id;
    }
}
