using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BikeHotel.GiantLeap.Models;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Internals;

namespace BikeHotel.GiantLeap
{
    [Preserve(AllMembers = true)]
    public class GiantLeapApiService : IGiantLeapApiService
    {

        private HttpClient GetClientWithDefaultHeaders()
        {
            var c = new HttpClient();
            c.DefaultRequestHeaders.Add("X-PartnerId", "banenor");
            c.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Android/Cardboard(banenor-3.4.5)/1.3.7");
            return c;
        }

        public async Task<string> ExchangeRefreshTokenAsync(string clientId, string refreshToken)
        {
            var json = new JObject();
            json["clientIdentifier"] = clientId;
            json["refreshToken"] = refreshToken;
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var res = await GetClientWithDefaultHeaders().PostAsync(GiantLeapPaths.ReAuthUrl, content);
            if (res.IsSuccessStatusCode)
            {
                var resultString = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CodeVerificationResult>(resultString);
                return result.AccessToken;
            }
            Analytics.TrackEvent("Refresh token exchange failed", new Dictionary<string, string> { { "StatusCode", res.StatusCode.ToString() } });
            return null;
        }

        public async Task<IList<Permit>> GetMyPermitsAsync(string phoneNumber, string accessToken)
        {
            var json = new JObject();
            json["phoneNumber"] = phoneNumber;
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var client = GetClientWithDefaultHeaders();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Token", accessToken);
            var res = await client.PostAsync(GiantLeapPaths.MyPermitsUrl, content);
            if (res.IsSuccessStatusCode)
            {
                var resultString = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MyPermitsResult>(resultString);
                return result.Permits;
            }
            Analytics.TrackEvent("Get my permits failed", new Dictionary<string, string> { { "StatusCode", res.StatusCode.ToString() } });
            return null;
        }

        public async Task<CodeVerificationResult> RequestAccessTokenFromVerificationCodeAsync(string phoneNumber, string verificationCode)
        {
            var json = new JObject();
            json["phoneNumber"] = phoneNumber;
            json["code"] = verificationCode;
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var res = await GetClientWithDefaultHeaders().PostAsync(GiantLeapPaths.VerifyCodeUrl, content);
            if (res.IsSuccessStatusCode)
            {
                var resultString = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CodeVerificationResult>(resultString);
                return result;
            }
            Analytics.TrackEvent("Access token exchange failed", new Dictionary<string, string> { { "StatusCode", res.StatusCode.ToString() } });
            return null;
        }

        public async Task<bool> RequestVerificationCodeAsync(string phoneNumber)
        {
            var json = new JObject();
            json["phoneNumber"] = phoneNumber;
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var res = await GetClientWithDefaultHeaders().PostAsync(GiantLeapPaths.RequestVerificationCodeUrl, content);
            if (res.IsSuccessStatusCode)
                return true;
            Analytics.TrackEvent("SMS code request failed", new Dictionary<string, string> { { "StatusCode", res.StatusCode.ToString() } });
            return false;
        }

        public async Task<HttpStatusCode> UnlockAsync(string permitId, string accessToken)
        {
            var json = new JObject();
            json["action"] = "UNLOCK";
            json["permitId"] = permitId;
            json["formData"] = new JArray();
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var client = GetClientWithDefaultHeaders();
            client.DefaultRequestHeaders.Add("X-Token", accessToken);
            var res = await client.PostAsync(GiantLeapPaths.UnlockUrl, content);
            Analytics.TrackEvent("Unlock request result", new Dictionary<string, string> { { "StatusCode", res.StatusCode.ToString() }, { "SatusMessage", res.ReasonPhrase } });
            return res.StatusCode;
        }
    }
}
