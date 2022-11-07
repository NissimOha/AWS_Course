using Arineta.Aws.Common.Entities;
using Arineta.Aws.Dto;
using AutoMapper;

namespace Arineta.Aws.Service.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
            CreateMap<RoleType, RoleTypeDto>();
            CreateMap<RoleTypeDto, RoleType>();
        }
    }
}
