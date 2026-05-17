using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;


namespace Aplikacija.Models
{
    public class Korisnik : IdentityUser
    {


        public string Ime { get; set; }
        public string Prezime { get; set; }
        public bool ClanarinaAktivna { get; set; }
    }
}
