using EF.context;
using EF.service.@interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.service.impl
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly NeondbContext context;

        public RoleServiceImpl(NeondbContext context)
        {
            this.context = context;
        }
        public Role GetAdminRole()
        {
            return context.Roles.Find(3L);
        }

        public Role GetDoctorRole()
        {
            return context.Roles.Find(1L);
        }

        public Role GetPatientRole()
        {
            return context.Roles.Find(2L);
        }
    }
}
