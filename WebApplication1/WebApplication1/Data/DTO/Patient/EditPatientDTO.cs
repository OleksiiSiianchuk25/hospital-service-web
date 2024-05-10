using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.DTO.User.Validity;

namespace WebApplication1.Data.DTO.Patient
{
    public class EditPatientDTO
    {

        [Required]
        public long UserId { get; set; }
        [Required]
        [UniqueEmail]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        public EditPatientDTO() { }

    }

}
