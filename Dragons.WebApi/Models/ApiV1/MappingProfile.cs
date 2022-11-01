using AutoMapper;
using Dragons.Models.Dragons;
using Dragons.WebApi.Models.ApiV1.Worlds;

namespace Dragons.WebApi.Models.ApiV1
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
