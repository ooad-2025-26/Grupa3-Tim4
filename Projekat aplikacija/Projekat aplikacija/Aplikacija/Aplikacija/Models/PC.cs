using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class PC : Uredjaj

    {

        public string Naziv { get; set; }
        public double CijenaPoSatu { get; set; }

        public string Model { get; set; }


        public string GetNaziv() => Naziv;
        public double GetCijenaPoSatu() => CijenaPoSatu;

        public string GetModel() => Model;
    }
}