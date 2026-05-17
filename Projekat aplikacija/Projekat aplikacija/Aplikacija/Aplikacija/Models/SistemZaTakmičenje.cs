using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class SistemZaTakmicenje
    {
        [Key]
        public int Id { get; set; }

        public string ApiUrl { get; set; }
        public int IntervalOsvjezavanja { get; set; }
    }
}
