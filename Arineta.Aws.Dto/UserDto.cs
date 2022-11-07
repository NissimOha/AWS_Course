using System;

namespace Arineta.Aws.Dto
{
    public record UserDto(Guid Id, string FirstName, string LastName, AddressDto Address, RoleTypeDto Role)
    {
    }
}
