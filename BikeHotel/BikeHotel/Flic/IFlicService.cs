using System;
using System.Collections.Generic;
using System.Text;

namespace BikeHotel.Flic
{
    public interface IFlicService
    {
        void SetAppCredentialsFromSettings();
        bool GrabButton();
        //TODO: Disconnect
        //TODO: Check connection
    }
}
