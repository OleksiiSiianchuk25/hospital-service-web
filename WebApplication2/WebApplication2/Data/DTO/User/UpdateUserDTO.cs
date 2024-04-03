using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DTO.User
{
    public class UpdateUserDTO
    {
        public long UserId { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Patronymic { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Type { get; set; } = null!;

        //Doctor
        public UpdateUserDTO(long userId, string email, string firstName, string lastName, string patronymic, string phone, string type)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Phone = phone;
            Type = type;
        }


        //Patient
        public UpdateUserDTO(long userId, string email, string firstName, string lastName, string phone)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }
    }
}
