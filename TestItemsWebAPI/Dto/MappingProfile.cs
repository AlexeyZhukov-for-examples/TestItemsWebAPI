using AutoMapper;
using TestItemsWebAPI.Entities;

namespace TestItemsWebAPI.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemDto, Item>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
