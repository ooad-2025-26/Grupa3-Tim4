using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Uredjaj
    {
        [Key]
        public int Id { get; set; }

        [EnumDataType(typeof(StatusUredjaja))]
        public StatusUredjaja Status { get; set; }
        public int GetId() => Id;
    }
}
