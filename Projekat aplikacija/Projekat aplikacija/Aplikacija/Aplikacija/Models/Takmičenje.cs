using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Takmicenje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters long!")]
        [RegularExpression(@"^[0-9a-zA-ZšđčćžŠĐČĆŽ ]+$", ErrorMessage = "Name can only contain letters, numbers and spaces!")]
        [DisplayName("Name: ")]
        public string Naziv { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = "Game name must be between 2 and 30 characters long!")]
        [RegularExpression(@"^[0-9a-zA-ZšđčćžŠĐČĆŽ ]+$", ErrorMessage = "Game can only contain letters, numbers and spaces!")]
        [DisplayName("Game: ")]
        public string Igra { get; set; }

        [Required]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31",
    ErrorMessage = "Date must be between 2020 and 2100!")]
        [DisplayName("Date: ")]
        public DateTime Datum { get; set; }

        public int GetId() => Id;
    }
}
