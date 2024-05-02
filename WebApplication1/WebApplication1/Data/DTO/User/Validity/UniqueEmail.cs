using EF.service;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.DTO.User.Validity
{
    public class UniqueEmail :ValidationAttribute
    {
       

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));
            if (userService == null)
            {
                throw new InvalidOperationException("IUserService service is not available.");
            }

            var email = value as string;
            Console.WriteLine(email);
            try
            {
                Console.WriteLine("sdc");
                var user = userService.FindByEmail(email);
                

            }
            catch (Exception ex)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Email is already in use."); ;
        }
    }
}
