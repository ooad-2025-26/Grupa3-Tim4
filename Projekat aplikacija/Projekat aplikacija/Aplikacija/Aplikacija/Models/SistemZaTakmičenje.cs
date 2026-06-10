using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class SistemZaTakmicenje
    {
        [Key]
        public int Id { get; set; }

        public string NazivSistema { get; set; }

        public string ApiUrl { get; set; }
        public int IntervalOsvjezavanja { get; set; }
    }
}
