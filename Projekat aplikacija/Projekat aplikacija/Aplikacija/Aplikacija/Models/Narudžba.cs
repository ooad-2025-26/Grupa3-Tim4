using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 50 characters long!")]
        [RegularExpression(@"^[0-9a-zA-ZšđčćžŠĐČĆŽ ]+$", ErrorMessage = "Product can only contain letters, numbers and spaces!")]
        [DisplayName("Product: ")]
        public string Proizvod { get; set; }


        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0!")]
        [DisplayName("Price: ")]
        public double Cijena { get; set; }


        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "Status must be between 3 and 20 characters long!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Status can only contain letters and spaces!")]
        [DisplayName("Status: ")]
        public string Status { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Sesija")]
        public int SesijaId { get; set; }
        public Sesija Sesija { get; set; }

        public int GetId() => Id;
    }
}
