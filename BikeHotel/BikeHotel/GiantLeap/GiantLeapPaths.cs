using System;
using System.Collections.Generic;
using System.Text;

namespace BikeHotel.GiantLeap
{
    public static class GiantLeapPaths
    {
        private static readonly string BaseApiUrl = "https://parko.giantleap.no/";
        public static Uri RequestVerificationCodeUrl
        {
            get
            {
                return new Uri(BaseApiUrl + "/client/suc-request");
            }
        }
        public static Uri VerifyCodeUrl
        {
            get
            {
                return new Uri(BaseApiUrl + "/client/suc-verify");
            }
        }
        public static Uri MyPermitsUrl
        {
            get
            {
                return new Uri(BaseApiUrl + "/permit/list");
            }
        }
        public static Uri ReAuthUrl
        {
            get
            {
                return new Uri(BaseApiUrl + "/client/reauth");
            }
        }

        public static Uri UnlockUrl
        {
            get
            {
                return new Uri(BaseApiUrl + "/permit/update");
            }
        }
    }
}
