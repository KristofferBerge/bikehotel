using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Internals;

namespace BikeHotel.GiantLeap.Models
{
    [Preserve(AllMembers = true)]
    public class CodeVerificationResult
    {
        [JsonProperty("token")]
        public string AccessToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
