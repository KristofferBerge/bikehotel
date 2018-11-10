using BikeHotel.GiantLeap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BikeHotel.Unlock
{
    public static class UnlockService
    {
        private static UserService UserService = DependencyService.Get<UserService>();
        private static IGiantLeapApiService ApiService = DependencyService.Get<IGiantLeapApiService>();
        public static async Task<bool> TryUnlock()
        {
            try
            {
                var resultCode = await ApiService.UnlockAsync(UserService.DefaultPermitId, await UserService.GetCachedAccessTokenAsync());
                if (resultCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    // Try refreshing token
                    var newToken = await ApiService.ExchangeRefreshTokenAsync(UserService.UserId, await UserService.GetCachedRefreshTokenAsync());
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        UserService.SetCachedAccessTokenAsync(newToken);
                        resultCode = await ApiService.UnlockAsync(UserService.DefaultPermitId, await UserService.GetCachedAccessTokenAsync());
                    }
                }
                if (resultCode != System.Net.HttpStatusCode.OK)
                    //TODO: Sign user out
                    return false;
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
