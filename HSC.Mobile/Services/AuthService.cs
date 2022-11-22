using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSC.Mobile.Services
{
    public class AuthService
    {
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpire { get; set; }
        public string RefreshToken { get; set; }

        public async Task<string> GetStoredToken()
        {
            if (!string.IsNullOrEmpty(AccessToken) && DateTimeOffset.UtcNow > AccessTokenExpire)
                await RefreshAccessToken();

            return AccessToken;
        }

        public async Task<bool> GetAndSaveToken(string userName, string password)
        {
            var client = new HttpClient();

            var body = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("username", userName),
                new("password", password),
                new("client_id", "hsc-mobile"),
                new("client_secret", "ikmY3iC4EQ5bJ4xyOgPD4BKsdtCZ5t8F"),
            };

            try
            {
                var result = await client.PostAsync($"http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.io:8080/auth/realms/chess/protocol/openid-connect/token", new FormUrlEncodedContent(body));

                if (!result.IsSuccessStatusCode) return false;

                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TokenResponseBody>(content);

                AccessToken = response.AccessToken;
                RefreshToken = response.RefreshToken;
                AccessTokenExpire = DateTimeOffset.UtcNow.AddSeconds(response.AccessTokenExpires);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task RefreshAccessToken()
        {
            var client = new HttpClient();

            var body = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", "hsc-mobile"),
                new("client_secret", "ikmY3iC4EQ5bJ4xyOgPD4BKsdtCZ5t8F"),
                new("refresh_token", RefreshToken),
            };

            var result = await client.PostAsync($"http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.io:8080/auth/realms/chess/protocol/openid-connect/token", new FormUrlEncodedContent(body));

            var response = JsonConvert.DeserializeObject<TokenResponseBody>(await result.Content.ReadAsStringAsync());

            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
            AccessTokenExpire = DateTimeOffset.UtcNow.AddSeconds(response.AccessTokenExpires);
        }
    }

    public class TokenResponseBody
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int AccessTokenExpires { get; set; }
        [JsonProperty("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
