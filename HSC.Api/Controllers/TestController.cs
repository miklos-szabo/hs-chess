using HSC.Common.RequestContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly IRequestContext _requestContext;

        public TestController(IRequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        [HttpGet]
        public string GetHello()
        {
            return "Hello!";
        }
    }
}
