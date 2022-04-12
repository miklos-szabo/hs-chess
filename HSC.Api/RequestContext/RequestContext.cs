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

            UserName = _httpContext.Request.Headers.TryGetValue("UserName", out var userNameResults) ? userNameResults.First() : string.Empty;

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, UserName) };
            var identity = new ClaimsIdentity(claims, "asd");

            _httpContext.User.AddIdentity(identity);
        }
    }
}
