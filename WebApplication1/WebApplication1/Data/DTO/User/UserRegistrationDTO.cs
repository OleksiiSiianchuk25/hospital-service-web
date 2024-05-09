using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.DTO.User.Validity;

namespace WebApplication1.Data.DTO.User
{
    public class UserRegistrationDTO
    {
        [Required]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; } 

        public UserRegistrationDTO(string email, string firstName, string lastName, string phone, string password)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Password = password;
        }

        public UserRegistrationDTO() { }
    }
}
