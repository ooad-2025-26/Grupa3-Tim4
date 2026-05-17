using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Uredjaj
    {
        [Key]
        public int Id { get; set; }
        public StatusUredjaja Status { get; set; }
        public int GetId() => Id;
    }
}
