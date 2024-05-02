using EF;
using EF.DTO.User;
using EF.service;
using EF.service.@interface;
using Microsoft.AspNetCore.Mvc;
using NLog;
using WebApplication1.Data;
using WebApplication1.Data.DTO.User;

namespace WebApplication1.Controllers
{
    [Route("api")]

    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly Logger logger;

        public UserController(IUserService userService,IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            logger.Info("Returning Login page");
            return View();
        }

        [HttpGet]
        [Route("registration")]
        public IActionResult Registration()
        {
            logger.Info("Returning Registration page");
            return View(new UserRegistrationDTO());
        }

        [HttpPost]
        [Route("registration")]
        public IActionResult Registration(UserRegistrationDTO user)
        {
            if (!ModelState.IsValid)
            {
                logger.Warn("User fields are invalid");

                return BadRequest(ModelState);
            }
            else
            {
                userService.RegisterPatient(user);
                logger.Info("Success registariton new Patient");
            }

            logger.Info("Returning Login page");
            return Redirect("/api/login");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(
            [FromForm] string email,
            [FromForm] string password
            )
        {
            User user;
            try
            {
                user = userService.FindByEmail(email);
            }
            catch (Exception ex)
            {
                logger.Warn($"User with email:{email} not found");
                return View();
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!passwordMatch)
            {
                logger.Warn($"Incorrect password for user{email}");
                return View();
            }
            else
            {
                Context.UserId = user.UserId;
                logger.Info($"Setted new UserId:{user.UserId}");
                if (user.RoleRef == roleService.GetPatientRole().RoleId)
                {
                    logger.Info("Redirecting to patient page");
                    return Redirect("/patient");
                } 
                else if(user.RoleRef == roleService.GetDoctorRole().RoleId)
                {
                    logger.Info("Redirecting to doctor page");
                    return Redirect("/doctor");
                }
                else
                {
                    logger.Info("Redirecting to admin page");
                    return Redirect("/admin");
                }
            }
        }

        [HttpGet]
        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            logger.Info("Returning ForgotPassword page");
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword([FromForm] string email)
        {
            try
            {
                userService.ChangePasswordByEmail(email);
            }
            catch (Exception ex)
            {
                logger.Warn($"User with email:{email} not found");
                return View();
            }
            logger.Info($"Password for user:{email} was successfully changed");
            logger.Info("Returning Login page");

            return Redirect("/api/login");
        }
    }
}
