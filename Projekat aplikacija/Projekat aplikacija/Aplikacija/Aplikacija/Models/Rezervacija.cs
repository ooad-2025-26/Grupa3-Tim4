namespace Aplikacija.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31",
    ErrorMessage = "Start time must be between 2020 and 2100!")]
        [DisplayName("Start time: ")]
        public DateTime VrijemeOd { get; set; }

        [Required]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31",
            ErrorMessage = "End time must be between 2020 and 2100!")]
        [DisplayName("End time: ")]
        public DateTime VrijemeDo { get; set; }

        [EnumDataType(typeof(StatusRezervacije))]
        public StatusRezervacije Status { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Uredjaj")]
        public int UredjajId { get; set; }
        public Uredjaj Uredjaj { get; set; }

        public int GetId() => Id;
    }
}
