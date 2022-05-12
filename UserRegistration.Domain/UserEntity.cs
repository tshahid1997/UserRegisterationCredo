using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Domain.Common;

namespace UserRegistration.Domain
{
    public class UserEntity: AuditableEntity
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PersonalNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
