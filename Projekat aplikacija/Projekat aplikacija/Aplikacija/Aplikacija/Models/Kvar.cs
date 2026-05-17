using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Kvar
    {
        [Key]
        public int Id { get; set; }

        public string Opis { get; set; }
        public StatusKvara Status { get; set; }

        [ForeignKey("Uredjaj")]
        public int UredjajId { get; set; }
        public Uredjaj Uredjaj { get; set; }

        public int GetId() => Id;
        public string GetOpis() => Opis;
    }
}
