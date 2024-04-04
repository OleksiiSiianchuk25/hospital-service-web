namespace EF.DTO.User
{
    public class UserDTO
    {
        public long UserId { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Patronymic { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Password { get; set; } = null!;

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

        public UserDTO(string email, string firstName, string lastName, string phone, string password)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Password = password;
        }
    }
}
