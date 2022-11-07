using System;

namespace Arineta.Aws.Common.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Address Address { get; set; }
        public RoleType Role { get; set; }
    }
}
