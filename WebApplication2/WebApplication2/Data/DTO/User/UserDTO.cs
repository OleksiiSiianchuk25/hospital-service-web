using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Phone = phone;
            Password = password;
            Type = type;
        }

        public UserDTO()
        {
        }

        public UserDTO(string email, string firstName, string lastName, string phone, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Password = password;
        }
    }
}
