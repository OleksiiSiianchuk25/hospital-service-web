using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.DTO.User.Validity;

namespace EF.DTO.User
{
    public class UserDTO
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
        public string Patronymic { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Type { get; set; }

        public UserDTO(string email, string firstName, string lastName, string patronymic, string phone, string password, string type)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Patronymic = patronymic;
            this.Phone = phone;
            this.Password = password;
            this.Type = type;
        }

        public UserDTO()
        {
        }

/*        public UserDTO(string email, string firstName, string lastName, string phone, string password)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Password = password;
        }*/
    }
}
