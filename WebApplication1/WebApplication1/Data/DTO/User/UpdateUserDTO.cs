using System.ComponentModel.DataAnnotations;

namespace EF.DTO.User
{
    public class UpdateUserDTO
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Patronymic { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Type { get; set; } = null!;

        // Doctor
        public UpdateUserDTO(long userId, string email, string firstName, string lastName, string patronymic, string phone, string type)
        {
            this.UserId = userId;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Patronymic = patronymic;
            this.Phone = phone;
            this.Type = type;
        }

        // Patient
        public UpdateUserDTO(long userId, string email, string firstName, string lastName, string phone)
        {
            this.UserId = userId;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
        }

        public UpdateUserDTO() { }
    }
}
