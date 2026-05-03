using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class XBox
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }
        public double CijenaPoSatu { get; set; }

        public StatusUredjaja Status { get; set; }

        public string GetNaziv() => Naziv;
        public double GetCijenaPoSatu() => CijenaPoSatu;
    }
}