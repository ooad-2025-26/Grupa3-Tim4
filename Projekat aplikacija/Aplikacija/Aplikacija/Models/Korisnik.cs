using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public bool ClanarinaAktivna { get; set; }
    }
}
