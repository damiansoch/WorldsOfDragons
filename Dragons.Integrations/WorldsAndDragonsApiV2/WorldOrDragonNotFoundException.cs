using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    [Serializable]
    public class WorldOrDragonNotFoundException:ApiException
    {
        public WorldOrDragonNotFoundException()
        {

        }
        public WorldOrDragonNotFoundException(string message)
        {

        }
        public WorldOrDragonNotFoundException(string message, Exception inner):base(message, inner)
        {

        }
        protected WorldOrDragonNotFoundException(
            SerializationInfo info,
            StreamingContext context):base(info, context)
        {

        }
    }
}
