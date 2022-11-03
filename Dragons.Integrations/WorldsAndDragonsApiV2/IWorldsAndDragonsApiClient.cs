using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    public interface IWorldsAndDragonsApiClient
    {
        //world
        Task<World[]> GetWorlds(int? skip = null, int? take = null, CancellationToken? cancellationToken = null);
        Task<World> GetWorld(int worldId, CancellationToken? cancellationToken = null);
        Task<AddWorldResponce> AddWorld(AddWorldRequest request, CancellationToken? cancellationToken = null);
        Task UpdateWorld(int worldId, UpdateWorldRequest request, CancellationToken? cancellationToken = null);
        Task DeleteWorld(int worldId, CancellationToken? cancellationToken = null);

        //dragon
        //Task<Dragon[]> GetDragons(int? worldId,int? skip = null, int? take = null, CancellationToken? cancellationToken = null);
        //Task<Dragon> GetDragon(int worldId,int dragonId, CancellationToken? cancellationToken = null);
        //Task<AddDragonResponce> AddDragon(AddDragonRequest request, CancellationToken? cancellationToken = null);
        //Task UpdateDragon(int worldId,int dragonId, UpdateDragonRequest request, CancellationToken? cancellationToken = null);
        //Task DeleteDragon(int worldId,int dragonId, CancellationToken? cancellationToken = null);

        //image
        Task SetDragonImage(int worldId, int dragonId, byte[] image, string filename, CancellationToken? cancellationToken = null);
        Task DeleteDragonImage(int worldId, int dragonId, CancellationToken? cancellationToken = null);

    }
}
