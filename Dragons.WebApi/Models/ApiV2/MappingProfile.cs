using AutoMapper;
using Dragons.Models.Dragons;
using Dragons.WebApi.Models.ApiV2.Dragons;
using Dragons.WebApi.Models.ApiV2.Worlds;

namespace Dragons.WebApi.Models.ApiV2
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //world
            CreateMap<World, WorldDto>();
            CreateMap<AddWorldRequest, World>();
            CreateMap<UpdateWorldRequest, World>();
            //dragon
            CreateMap<Dragon, DragonDto>();
            CreateMap<AddDragonRequest, Dragon>();
            CreateMap<UpdateDragonInWorldRequest, Dragon>();
        }
    }
}
