using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class PlayStation : Uredjaj
    {
        public string Naziv { get; set; }
        public double CijenaPoSatu { get; set; }

        public string GetNaziv() => Naziv;
        public double GetCijenaPoSatu() => CijenaPoSatu;
    }
}
