using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Models.Dragons
{
    public class World
    {
        public World()
        {
            Dragons = new List<Dragon>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Dragon> Dragons { get; set; }

    }
}
