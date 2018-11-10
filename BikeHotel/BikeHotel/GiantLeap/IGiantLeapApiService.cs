using BikeHotel.GiantLeap.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BikeHotel.GiantLeap
{
    public interface IGiantLeapApiService
    {
        Task<bool> RequestVerificationCodeAsync(string phoneNumber);
        Task<CodeVerificationResult> RequestAccessTokenFromVerificationCodeAsync(string phoneNumber, string verificationCode);
        Task<IList<Permit>> GetMyPermitsAsync(string phoneNumber, string accessToken);
        Task<string> ExchangeRefreshTokenAsync(string userId, string refreshToken);
        Task<HttpStatusCode> UnlockAsync(string permitId, string accessToken);
    }
}
