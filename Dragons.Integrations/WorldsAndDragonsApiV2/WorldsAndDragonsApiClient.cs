using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    public class WorldsAndDragonsApiClient : IWorldsAndDragonsApiClient
    {
        public Task<World> GetWorld(int worldId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task<World[]> GetWorlds(int? skip = null, int? take = null, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task<AddWorldResponce> AddWorld(AddWorldRequest request, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task UpdateWorld(int worldId, UpdateWorldRequest request, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task DeleteWorld(int worldId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task SetDragonImage(int worldId, int dragonId, byte[] image, string filename, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        public Task DeleteDragonImage(int worldId, int dragonId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

    }
}
