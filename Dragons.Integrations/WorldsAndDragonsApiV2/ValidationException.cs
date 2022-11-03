using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    [Serializable]
    public class ValidationException:ApiException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public string[] ValidationErrors { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());
            stringBuilder.AppendLine("Validation Errors:");
            foreach (var error in ValidationErrors)
            {
                stringBuilder.AppendLine(error);
            }

            return stringBuilder.ToString();
        }
    }
}
