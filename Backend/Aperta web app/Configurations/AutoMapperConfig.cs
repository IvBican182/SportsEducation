using Aperta_web_app.Data;
using Aperta_web_app.Models.Club;
using Aperta_web_app.Models.Group;
using AutoMapper;

namespace Aperta_web_app.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Club, CreateClubDto>().ReverseMap();
            CreateMap<Club, GetClubsDto>().ReverseMap();
            CreateMap<Club, ClubDto>().ReverseMap();
            CreateMap<Club, UpdateClubDto>().ReverseMap();

            CreateMap<Group, CreateGroupDto>().ReverseMap();
            CreateMap<Group, GetGroupsDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<Group, UpdateGroupDto>().ReverseMap();

            

        }
    }
}
