using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BikeHotel.Flic;
using BikeHotel.GiantLeap;
using BikeHotel.Unlock;
using IO.Flic.Lib;
using Xamarin.Forms;

namespace BikeHotel.Droid.Flic
{
    [BroadcastReceiver(Enabled = true, Exported = true), IntentFilter(actions: new string[] { "io.flic.FLICLIB_EVENT" })]
    public class UnlockBroadcastReceiver : FlicBroadcastReceiver
    {
        protected override void OnRequestAppCredentials(Context p0)
        {
            DependencyService.Get<IFlicService>().SetAppCredentialsFromSettings();
        }

        public async override void OnButtonUpOrDown(Context context, FlicButton button, bool wasQueued, int timeDiff, bool isUp, bool isDown)
        {
            var userService = DependencyService.Get<UserService>();
            await UnlockService.TryUnlock();
            base.OnButtonUpOrDown(context, button, wasQueued, timeDiff, isUp, isDown);
        }
    }
}