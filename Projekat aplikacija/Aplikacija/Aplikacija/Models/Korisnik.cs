using Microsoft.AspNetCore.Identity;

namespace Aplikacija.Models
{
    public class Korisnik : IdentityUser
    {
        public bool ClanarinaAktivna { get; set; }
    }
}