using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Models.Dragons
{
    public class Dragon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public byte[] DragonImageFile { get; set; }

        public string DragonImageFileName { get; set; }

    }
}
