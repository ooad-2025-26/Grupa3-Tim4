using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Sesija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31",
    ErrorMessage = "Start time must be between 2020 and 2100!")]
        [DisplayName("Start time: ")]
        public DateTime VrijemePocetka { get; set; }

        [Required]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31",
            ErrorMessage = "End time must be between 2020 and 2100!")]
        [DisplayName("End time: ")]
        public DateTime VrijemeZavrsetka { get; set; }

        [EnumDataType(typeof(StatusSesije))]
        public StatusSesije Status { get; set; }

        [ForeignKey("Uredjaj")]
        public int UredjajId { get; set; }
        public Uredjaj Uredjaj { get; set; }

        [ForeignKey("Korisnik")]
        public string? KorisnikId { get; set; }
        public Korisnik? Korisnik { get; set; }

        public bool AnonimniGost { get; set; }

        [ForeignKey("Takmicenje")]
        public int? TakmicenjeId { get; set; }
        public Takmicenje? Takmicenje { get; set; }
        public ICollection<Placanje>? Placanja { get; set; }
        public int GetId() => Id;
        public DateTime GetVrijemePocetka() => VrijemePocetka;
        public StatusSesije GetStatus() => Status;
    }
}
