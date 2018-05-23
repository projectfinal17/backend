using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace API.Entities
{
    public class RoleEntity : IdentityRole<Guid>
    {
        public RoleEntity()
            : base()
        { }

        public RoleEntity(string roleName)
            : base(roleName)
        { }
    }
}
