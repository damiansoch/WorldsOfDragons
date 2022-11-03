using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    [Serializable]
    public class AuthorizationException:ApiException
    {
        public AuthorizationException()
        {

        }
        public AuthorizationException(string message)
        {

        }
        public AuthorizationException(string message, Exception inner) : base(message, inner)
        {

        }
        protected AuthorizationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {

        }
    }
}
