using Microsoft.AspNetCore.Authorization;

namespace Dragons.WebApi.Attributes
{
    public class BasicAuthorizationAttribute:AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
