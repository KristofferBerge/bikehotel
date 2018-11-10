using System;
using System.Collections.Generic;
using System.Text;

namespace BikeHotel.Flic
{
    public interface IFlicService
    {
        void SetAppCredentialsFromSettings();
        bool GrabButton();
        //TODO: Connect
        //TODO: Disconnect
        //TODO: Check connection
    }
}
