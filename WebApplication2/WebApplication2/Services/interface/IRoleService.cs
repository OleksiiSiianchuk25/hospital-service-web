namespace EF.service.@interface
{
    public interface IRoleService
    {
        Role GetPatientRole();
        Role GetDoctorRole();
        Role GetAdminRole();
    }
}
