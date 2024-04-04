using EF.context;
using EF.service.@interface;

namespace EF.service.impl
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly NeondbContext context;

        public RoleServiceImpl(NeondbContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public Role GetAdminRole()
        {
            return this.context.Roles.Find(3L);
        }

        /// <inheritdoc/>
        public Role GetDoctorRole()
        {
            return this.context.Roles.Find(1L);
        }

        /// <inheritdoc/>
        public Role GetPatientRole()
        {
            return this.context.Roles.Find(2L);
        }
    }
}
