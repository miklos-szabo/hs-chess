using System.Linq;
using System.Security.Claims;
using HSC.Common.RequestContext;
using Microsoft.AspNetCore.Http;

namespace OnlineAuction.Api.RequestContext
{
    public class RequestContext : IRequestContext
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        private readonly HttpContext _httpContext;

        public RequestContext(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext.HttpContext;

            UserName = _httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Name = _httpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            Email = _httpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        }
    }
}
