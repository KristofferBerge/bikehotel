using BikeHotel.GiantLeap.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace BikeHotel
{
    public class UserService
    {
        [Preserve]
        public UserService() { }
        public bool IsUserConfigured
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                    return false;
                if (string.IsNullOrEmpty(DefaultPermitId))
                    return false;
                // TODO: Add more cases to trigger onboarding
                return true;
            }
        }

        public async Task ClearAllDataAndSignOutAsync()
        {
            await SetCachedAccessTokenAsync("");
            await SetCachedRefreshTokenAsync("");
            Preferences.Clear();
            ((App)App.Current).SetMainPage();
        }

        public string UserId
        {
            get
            {
                return Preferences.Get("userId", string.Empty);
            }
            set
            {
                Preferences.Set("userId", value);
            }
        }

        public string PhoneNumber
        {
            get
            {
                return Preferences.Get("phoneNumber", string.Empty);
            }
            set
            {
                Preferences.Set("phoneNumber", value);
            }
        }
        public string DefaultPermitId
        {
            get
            {
                return Preferences.Get("defaultPermitId", string.Empty);
            }
            set
            {
                Preferences.Set("defaultPermitId", value);
            }
        }

        public IList<Permit> Permits
        {
            get
            {
                var rawString = Preferences.Get("rawPermitsString", string.Empty);
                return JsonConvert.DeserializeObject<List<Permit>>(rawString);
            }
            set
            {
                var rawString = JsonConvert.SerializeObject(value);
                Preferences.Set("rawPermitsString", rawString);
            }
        }

        public async Task<string> GetCachedAccessTokenAsync()
        {
            return await SecureStorage.GetAsync("accesstoken");
        }

        public async Task SetCachedAccessTokenAsync(string accessToken)
        {
            await SecureStorage.SetAsync("accesstoken", accessToken);
        }

        public async Task<string> GetCachedRefreshTokenAsync()
        {
            return await SecureStorage.GetAsync("refreshtoken");
        }

        public async Task SetCachedRefreshTokenAsync(string refreshToken)
        {
            await SecureStorage.SetAsync("refreshtoken", refreshToken);
        }

    }
}
