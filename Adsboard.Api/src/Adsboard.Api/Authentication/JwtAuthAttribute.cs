using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Adsboard.Api.Authentication
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}
