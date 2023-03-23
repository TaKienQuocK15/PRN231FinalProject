using AutoMapper;
using API.Models;

namespace API.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDTO>()
                .ForMember(dest =>
                dest.Teacher,
                opt => opt.MapFrom(src => src.Teacher.Name));
        }
    }
}
