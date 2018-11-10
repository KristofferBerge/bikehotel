using BikeHotel.GiantLeap.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace BikeHotel.GiantLeap.Mock
{
    public class GiantLeapMockApiService : IGiantLeapApiService
    {
        [Preserve]
        public GiantLeapMockApiService() { }
        public Task<string> ExchangeRefreshTokenAsync(string userId, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Permit>> GetMyPermitsAsync(string phoneNumber, string accessToken)
        {
            await Task.Delay(2500);
            return new List<Permit>(){
                new Permit
                {
                    Id = "qeoigwåoeirgaåoier",
                    Location = "Lillestrøm Messesiden",
                    PermitType = "BIKE_PARKING",
                    PermitTypeName = "Sykkelhotell"
                }
            };
        }


        public async Task<bool> RequestVerificationCodeAsync(string phoneNumber)
        {
            await Task.Delay(2500);
            return true;
        }

        public async Task<CodeVerificationResult> RequestAccessTokenFromVerificationCodeAsync(string phoneNumber, string verificationCode)
        {
            await Task.Delay(2500);
            return new CodeVerificationResult()
            {
                AccessToken = "asdfagaerarsgaegaefasefae",
                RefreshToken = "asdøgaheoiaoeiaewo"
            };
        }

        public async Task<HttpStatusCode> UnlockAsync(string permitId, string accessToken)
        {
            await Task.Delay(2500);
            return HttpStatusCode.OK;
        }
    }
}
