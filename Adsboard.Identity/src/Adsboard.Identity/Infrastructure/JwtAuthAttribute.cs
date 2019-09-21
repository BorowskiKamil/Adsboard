using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Adsboard.Identity.Infrastructure
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}
