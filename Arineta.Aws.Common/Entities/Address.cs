using System;

namespace Arineta.Aws.Common.Entities
{
    public class Address
    {
        //One to one relationship EF Core
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HomeNumber { get; set; }
    }
}
