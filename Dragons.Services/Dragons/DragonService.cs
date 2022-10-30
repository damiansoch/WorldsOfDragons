using Dragons.Models.Dragons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Services.Dragons
{
    public class DragonService : IDragonService
    {
        //----------------------------------------------------------world
        public bool DoesWorldExist(int id)
        {
            return Worlds.Any(o => o.Id == id);
        }
        public int GetWorldCount()
        {
            return Worlds.Count;
        }
        public int AddWorld(World world)
        {
            var id = Worlds.Select(o => o.Id).Max() + 1;
            world.Id = id;
            Worlds.Add(world);

            if (world.Dragons.Any())
            {
                var maxDragonId = Worlds.SelectMany(o => o.Dragons).Max(o => o.Id);
                foreach (var dragon in world.Dragons)
                {
                    dragon.Id = ++maxDragonId;
                }
            }
            return id;
        }
        public void DeleteWorld(int id)
        {
            var world = GetWorld(id);
            Worlds.Remove(world);
        }
        public World? GetWorld(int id)
        {
            return Worlds.SingleOrDefault(o => o.Id == id);
        }
        public void UpdateWorld(World world)
        {
            var worldToUpdate = GetWorld(world.Id);
            worldToUpdate.Name = world.Name;
        }
        public World?[] GetWorldList(int skip, int take, string? search)
        {
            return Worlds.OrderBy(o => o.Id)
                .Where(o=>string.IsNullOrWhiteSpace(search)
                || (o.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                ))
                .Skip(skip).Take(take).ToArray();
        }

        //------------------------------------------------------------------------dragon

        public bool DoesDragonExistInWorld(int worldId, int dragonId)
        {
            return Worlds.Any(o => o.Id == worldId && o.Dragons.Any(s => s.Id == dragonId));
        }
        public int GetDragonCount(int worldId)
        {
            return Worlds.Where(o => o.Id == worldId).Sum(o => o.Dragons.Count);
        }
        public int AddDragonToWorld(int worldId,Dragon dragon)
        {
            var world = GetWorld(worldId);
            var maxDragonId = Worlds.SelectMany(o => o.Dragons).Max(o => o.Id);
            dragon.Id = maxDragonId + 1;
            world.Dragons.Add(dragon);
            return dragon.Id;
        }
        public void DeleteDragonFromWorld(int worldId, int dragonId)
        {
            var world = GetWorld(worldId);
            var dragon = world.Dragons.Single(o=>o.Id == dragonId);
            world.Dragons.Remove(dragon);
        }
        public Dragon? GetDragonInWorld(int worldId, int dragonId)
        {
            return GetWorld(worldId).Dragons.SingleOrDefault(o=>o.Id==dragonId);
        }
        public void UpdateDragonInWorld(int worldId, Dragon dragon)
        {
            var dragonToUpdate = GetDragonInWorld(worldId, dragon.Id);
            dragonToUpdate.Name = dragon.Name;
            dragonToUpdate.Description = dragon.Description;

        }
        public Dragon[] GetDragonsinWorldList(int worldId, int skip, int take)
        {
            var world = GetWorld(worldId);
            return world.Dragons.OrderBy(o=>o.Id).Skip(skip).Take(take).ToArray();
        }
        public void SetDragonImage(int worldId, int dragonId, byte[] image, string fileName)
        {
            var dragon = GetDragonInWorld(worldId, dragonId);
            dragon.DragonImageFile = image;
            dragon.DragonImageFileName = fileName;
        }

        //--------------------------------------------


        private static IList<World?> Worlds = new List<World?>
        {
            new World
            {
                Id = 0,
                Name="HouseOfDragons",
                Dragons = new List<Dragon>
                {
                    new Dragon
                    {
                        Id=0,
                        Name="Arrax",
                        Description="Test Arrax Description"
                    },
                    new Dragon
                    {
                        Id=1,
                        Name="Balerion",
                        Description="Test Balerion Description"
                    },
                    new Dragon
                    {
                        Id=2,
                        Name="Caraxes",
                        Description="Test Caraxes Description"
                    },
                    new Dragon
                    {
                        Id=3,
                        Name="Dreamfyre",
                        Description="Test Dreamfyre Description"
                    },
                    new Dragon
                    {
                        Id=4,
                        Name="Meleys",
                        Description="Test Meleys Description"
                    },
                    new Dragon
                    {
                        Id=5,
                        Name="Moondancer",
                        Description="Test Moondancer Description"
                    },
                    new Dragon
                    {
                        Id=6,
                        Name="Seasmoke",
                        Description="Test Seasmoke Description"
                    },
                    new Dragon
                    {
                        Id=7,
                        Name="Silverwing",
                        Description="Test Silverwing Description"
                    }
                }
            },new World
            {
                Id = 1,
                Name="Harry Potter",
                Dragons = new List<Dragon>
                {
                    new Dragon
                    {
                        Id=8,
                        Name="Antipodean Opaleye",
                        Description="Test Antipodean Opaleye Description"
                    },
                    new Dragon
                    {
                        Id=9,
                        Name="Chinese Fireball",
                        Description="Test Chinese Fireball Description"
                    },
                    new Dragon
                    {
                        Id=10,
                        Name="Common Welsh Green",
                        Description="Test Common Welsh Green Description"
                    },
                    new Dragon
                    {
                        Id=11,
                        Name="Hebridean Black",
                        Description="Test Hebridean Black Description"
                    },
                    new Dragon
                    {
                        Id=12,
                        Name="Hungarian Horntail",
                        Description="Test Hungarian Horntail Description"
                    },
                    new Dragon
                    {
                        Id=13,
                        Name="Norwegian Ridgeback",
                        Description="Test Norwegian Ridgeback Description"
                    },
                    new Dragon
                    {
                        Id=14,
                        Name="Peruvian Vipertooth",
                        Description="Test Peruvian Vipertooth Description"
                    },
                    new Dragon
                    {
                        Id=015,
                        Name="Romanian Longhorn",
                        Description="Test Romanian Longhorn Description"
                    }
                }
            }
        };
    }
}
