using EF.DTO.User;
using WebApplication1.Data.DTO.User;

namespace EF.service
{
    public interface IUserService
    {
        User FindById(long id);
        User FindByEmail(string email);
        void RegisterPatient(UserRegistrationDTO registerUser);
        void RegisterDoctor(UserDTO registerUser);
        void EditUser(UpdateUserDTO user);
        void DeleteById(long id);
        void ChangePasswordByEmail(string email);
        List<User> GetDoctors();
        List<User> GetPatients();
        long GetNumberOfDoctors();
        long GetNumberOfPatients();
    }
}
