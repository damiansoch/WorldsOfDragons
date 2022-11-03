using Dragons.Models.Dragons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Services.Dragons
{
    public interface IDragonService
    {
        //world
        bool DoesWorldExist(int id);
        int GetWorldCount();
        int AddWorld(World world);
        void DeleteWorld(int id);
        World? GetWorld(int id);
        void UpdateWorld(World world);
        World?[] GetWorldList(int skip, int take,string?search);

        //dragon
        bool DoesDragonExistInWorld(int worldId, int dragonId);
        int GetDragonCount(int worldId);
        int AddDragonToWorld(int worldId, Dragon dragon);
        void DeleteDragonFromWorld(int worldId,int dragonId);
        Dragon? GetDragonInWorld(int worldId, int dragonId);
        void UpdateDragonInWorld(int worldId,Dragon dragon);
        Dragon[] GetDragonsinWorldList(int worldId, int skip, int take,string search);
        void SetDragonImage(int worldId,int dragonId,byte[]? image,string? fileName);
    }
}
