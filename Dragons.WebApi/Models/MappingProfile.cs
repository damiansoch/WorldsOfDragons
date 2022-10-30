using AutoMapper;
using Dragons.Models.Dragons;
using Dragons.WebApi.Models.Worlds;

namespace Dragons.WebApi.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<World, WorldDto>();
            CreateMap<AddWorldRequest, World>();
            CreateMap<UpdateWorldRequest, World>();
        }
    }
}
