using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Sesija
    {
        [Key]
        public int Id { get; set; }

        public DateTime VrijemePocetka { get; set; }
        public DateTime VrijemeZavrsetka { get; set; }
        public StatusSesije Status { get; set; }

        [ForeignKey("Uredjaj")]
        public int UredjajId { get; set; }
        public Uredjaj Uredjaj { get; set; }

        public int GetId() => Id;
        public DateTime GetVrijemePocetka() => VrijemePocetka;
        public StatusSesije GetStatus() => Status;
    }
}
