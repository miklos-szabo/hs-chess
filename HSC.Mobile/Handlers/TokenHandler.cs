using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Services;

namespace HSC.Mobile.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly AuthService _authService;

        public TokenHandler(AuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", await _authService.GetStoredToken());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
