using Bogus;
using EF.context;
using EF.DTO.User;
using EF.service.@interface;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebApplication1.Data.DTO.User;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EF.service.impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly NeondbContext context;
        private readonly IRoleService roleService;

        public UserServiceImpl(NeondbContext context)
        {
            this.context = context;
            this.roleService = new RoleServiceImpl(context);
        }

        public UserServiceImpl(NeondbContext context, IRoleService roleService)
        {
            this.context = context;
            this.roleService = roleService;
        }

        /// <inheritdoc/>
        public User FindByEmail(string email)
        {
            User user = this.context.Users
                .Include(u => u.RoleRefNavigation)
                .FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new ApplicationException("User with email: " + email + " does not exist!");
            }

            return user;
        }

        /// <inheritdoc/>
        public User FindById(long id)
        {
            User user = this.context.Users.FirstOrDefault(user => user.UserId == id);
            if (user == null)
            {
                throw new ApplicationException("User with id: " + id + " does not exist!");
            }

            return user;
        }

        /// <inheritdoc/>
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
            newUser.RoleRefNavigation = this.roleService.GetDoctorRole();
            this.context.Users.Add(newUser);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void RegisterPatient(UserRegistrationDTO registerUser)
        {
            User newUser = new User();
            newUser.FirstName = registerUser.FirstName;
            newUser.LastName = registerUser.LastName;
            newUser.Email = registerUser.Email;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            newUser.Phone = registerUser.Phone;
            newUser.RoleRefNavigation = this.roleService.GetPatientRole();
            this.context.Users.Add(newUser);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteById(long id)
        {
            User userToDelete = this.FindById(id);
            this.context.Users.Remove(userToDelete);
            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public void EditUser(UpdateUserDTO user)
        {
            User updateUser = this.FindById(user.UserId);
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;
            updateUser.Phone = user.Phone;
            updateUser.Email = user.Email;
            if (user.Patronymic != null)
            {
                updateUser.Patronymic = user.Patronymic;
            }

            if (user.Type != null)
            {
                updateUser.Type = user.Type;
            }

            this.context.SaveChanges();
        }

        /// <inheritdoc/>
        public List<User> GetDoctors()
        {
            return this.context.Users
                .Where(user => user.RoleRef == this.roleService.GetDoctorRole().RoleId)
                .ToList();
        }

        /// <inheritdoc/>
        public List<User> GetPatients()
        {
            return this.context.Users
                .Where(user => user.RoleRef == this.roleService.GetPatientRole().RoleId)
                .ToList();
        }

        /// <inheritdoc/>
        public void ChangePasswordByEmail(string email)
        {
            User user = this.FindByEmail(email);
            string newPassword = (this.GeneratePassoword());
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            this.SendEmailViaGmail(email, newPassword);
            this.context.SaveChanges();
        }

        private string GeneratePassoword()
        {
            var faker = new Faker();
            return faker.Internet.Password(8, false, "^");
        }

        private void SendEmailViaGmail(string email, string password)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse("legendshospitalbimbimbambam@gmail.com"));
            mailMessage.To.Add(MailboxAddress.Parse(email));

            mailMessage.Subject = "Новий пароль від eHospital";
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Ваш новий пароль: " + password + "<p>Записуйте на листочок!</p>" };

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

        /// <inheritdoc/>
        public long GetNumberOfDoctors()
        {
            return this.context.Users
                .Where(user => user.RoleRef == this.roleService.GetDoctorRole().RoleId)
                .Count();
        }

        /// <inheritdoc/>
        public long GetNumberOfPatients()
        {
            return this.context.Users
               .Where(user => user.RoleRef == this.roleService.GetPatientRole().RoleId)
               .Count();
        }
    }
}
