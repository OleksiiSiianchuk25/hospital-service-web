using Bogus;
using EF.context;
using EF.DTO.User;
using EF.service.@interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using MailKit.Net.Smtp;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Mail;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EF.service.impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly NeondbContext context;
        private readonly IRoleService roleService;

        public UserServiceImpl(NeondbContext context, IRoleService roleService)
        {
            this.context = context;
            this.roleService = roleService;
        }

        public User FindByEmail(string email)
        {
            User user = context.Users
                .Include(u => u.RoleRefNavigation)
                .FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new ApplicationException("User with email: " + email + " does not exist!");
            }
            return user;
        }

        public User FindById(long id)
        {
            User user = context.Users.FirstOrDefault(user => user.UserId == id);
            if (user == null)
            {
                throw new ApplicationException("User with id: " + id + " does not exist!");
            }
            return user;
        }

        public void RegisterDoctor(UserDTO registerDoctor)
        {
            User newUser = new User();
            newUser.FirstName = registerDoctor.FirstName;
            newUser.LastName = registerDoctor.LastName;
            newUser.Email = registerDoctor.Email;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(registerDoctor.Password);
            newUser.Phone = registerDoctor.Phone;
            newUser.Patronymic = registerDoctor.Patronymic;
            newUser.Type = registerDoctor.Type;
            newUser.RoleRefNavigation = roleService.GetDoctorRole();
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public void RegisterPatient(UserDTO registerUser)
        {
            User newUser = new User();
            newUser.FirstName = registerUser.FirstName;
            newUser.LastName = registerUser.LastName;
            newUser.Email = registerUser.Email;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            newUser.Phone = registerUser.Phone;
            newUser.RoleRefNavigation = roleService.GetPatientRole();
            context.Users.Add(newUser);
            context.SaveChanges();
        }
        public void DeleteById(long id)
        {
            User userToDelete = FindById(id);
            context.Users.Remove(userToDelete);
            context.SaveChanges();
        }

        public void EditUser(UpdateUserDTO user)
        {
            User updateUser = FindById(user.UserId);
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;
            updateUser.Phone = user.Phone;
            updateUser.Email = user.Email;
            if (user.Patronymic != null)
            {
                updateUser.Patronymic = user.Patronymic;
            }
            if (user.Type != null){
                updateUser.Type = user.Type;
            }
            context.SaveChanges();
        }

        public List<User> GetDoctors()
        {
            return context.Users
                .Where(user => user.RoleRef == roleService.GetDoctorRole().RoleId)
                .ToList();
        }

        public List<User> GetPatients()
        {
            return context.Users
                .Where(user => user.RoleRef == roleService.GetPatientRole().RoleId)
                .ToList();
        }

        public void ChangePasswordByEmail(string email)
        {
            User user = FindByEmail(email);
            string newPassword = (GeneratePassoword());
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            SendEmailViaGmail(email, newPassword);
            context.SaveChanges();
        }

        private string GeneratePassoword()
        {
            var faker = new Faker();
            return faker.Internet.Password(8, false, "^");
        }
        private void SendEmailViaGmail(string email,string password)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse("legendshospitalbimbimbambam@gmail.com"));
            mailMessage.To.Add(MailboxAddress.Parse(email));


            mailMessage.Subject = "Новий пароль від eHospital";
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Ваш новий пароль: " + password + "<p>Записуйте на листочок!</p>"};
            

            using SmtpClient smtpClient = new SmtpClient();
            try
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                smtpClient.Authenticate("legendshospitalbimbimbambam@gmail.com", "uxjrrlzxmtypzrpq");
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();

            }
        }

        public long GetNumberOfDoctors()
        {
            return context.Users
                .Where(user => user.RoleRef == roleService.GetDoctorRole().RoleId)
                .Count();
        }

        public long GetNumberOfPatients()
        {
            return context.Users
               .Where(user => user.RoleRef == roleService.GetPatientRole().RoleId)
               .Count();
        }
    }
}
