using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{

    public class User : BaseEntity  
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }

}
