using BikeHotel.GiantLeap;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
                        await UserService.SetCachedAccessTokenAsync(newToken);
                        resultCode = await ApiService.UnlockAsync(UserService.DefaultPermitId, await UserService.GetCachedAccessTokenAsync());
                    }
                }
                if (resultCode != System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("Unlock failed", new Dictionary<string, string> { { "HttpErrorCode", resultCode.ToString() } });
                    await UserService.ClearAllDataAndSignOutAsync();
                    return false;
                }
                return true;

            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> { { "Reason", "Unlock failed due to exception" } });
                return false;
            }
        }
    }
}
