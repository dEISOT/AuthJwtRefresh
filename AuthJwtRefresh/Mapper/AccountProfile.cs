using AuthJwtRefresh.DTO;
using AuthJwtRefresh.Models;
using AutoMapper;

namespace AuthJwtRefresh.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterRequest, Account>();
        }
    }
}
