using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    public class AddWorld422Responses
    {
        public Errors Errors { get; set; }
    }
    public class Errors
    {
        public string[] Name { get; set; }
    }
}
