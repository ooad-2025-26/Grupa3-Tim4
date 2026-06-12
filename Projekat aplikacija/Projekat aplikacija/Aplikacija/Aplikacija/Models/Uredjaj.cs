using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Uredjaj
    {

        [NotMapped]
        public string TipUredjaja => GetType().Name;
        [Key]
        public int Id { get; set; }

        [EnumDataType(typeof(StatusUredjaja))]
        public StatusUredjaja Status { get; set; }
        public int GetId() => Id;
    }
}
