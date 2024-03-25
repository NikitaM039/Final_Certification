using AutoMapper;
using System.Text;
using WebApiLibr.Models.DTO;
using WebApiLibr.Models.EntitiesModel;

namespace WebApiLibr
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(y => Encoding.ASCII.GetBytes(y.Password)))
                .ForMember(dest => dest.Salt, opts => opts.Ignore())
                .ForMember(dest => dest.RoleId, opts => opts.MapFrom(y => y.Role))
                .ForMember(dest => dest.Role, opts => opts.Ignore());
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(y => y.Password.ToString()))
                .ForMember(dest => dest.Role, opts => opts.MapFrom(y => y.RoleId));

            CreateMap<Message, MessageDTO>(MemberList.Destination).ReverseMap();
        }
        
    }
}