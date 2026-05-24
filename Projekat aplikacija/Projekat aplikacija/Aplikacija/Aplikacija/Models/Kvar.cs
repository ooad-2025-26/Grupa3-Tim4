using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Aplikacija.Models
{
    public class Kvar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 50 characters long!")]
        [RegularExpression(@"^[0-9a-zA-Z ]+$", ErrorMessage = "Description can only contain letters, numbers, and spaces!")] 
        [DisplayName("Description: ")]
        public string Opis { get; set; }
        
        [EnumDataType(typeof(StatusKvara))]
        public StatusKvara Status { get; set; }

        [ForeignKey("Uredjaj")]
        public int UredjajId { get; set; }
        public Uredjaj Uredjaj { get; set; }

        [ForeignKey("Sesija")]
        public int SesijaId { get; set; }
        public Sesija Sesija { get; set; }
        public int GetId() => Id;
        public string GetOpis() => Opis;
    }
}
