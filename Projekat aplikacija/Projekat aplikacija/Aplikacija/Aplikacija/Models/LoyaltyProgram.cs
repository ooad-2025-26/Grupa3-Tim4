using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aplikacija.Models
{
    public class LoyaltyProgram
    {
        [Key]
        public int Id { get; set; }

        public int UkupniBodovi { get; set; }
        public double PopustPoBodu { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int GetUkupniBodovi() => UkupniBodovi;
    }
}
