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
using BikeHotel.Droid.Flic;
using BikeHotel.Flic;
using IO.Flic.Lib;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidFlicService))]

namespace BikeHotel.Droid.Flic
{
    public class AndroidFlicService : IFlicService
    {
        public AndroidFlicService()
        {
            SetAppCredentialsFromSettings();
        }

        public void SetAppCredentialsFromSettings()
        {
            FlicManager.SetAppCredentials(FlicCredentials.AppId, FlicCredentials.AppSecret, FlicCredentials.AppName);
        }

        public bool GrabButton()
        {
            try
            {
                FlicManager.GetInstance(Application.Context, new FlicManagerGrabButtonCallback() { });
                return true;
            }
            catch (FlicAppNotInstalledException e)
            {
                return false;
            }
        }
    }
    public class FlicManagerGrabButtonCallback : Java.Lang.Object, IFlicManagerInitializedCallback
    {
        public void OnInitialized(FlicManager manager)
        {
            manager.InitiateGrabButton(CrossCurrentActivity.Current.Activity);
        }
    }
    public class FlicManagerCatchButtonCallback : Java.Lang.Object, IFlicManagerInitializedCallback
    {
        private int RequestCode;
        private Result ResultCode;
        private Intent Data;
        public FlicManagerCatchButtonCallback(int requestCode, Result resultCode, Intent data)
        {
            RequestCode = requestCode;
            ResultCode = resultCode;
            Data = data;
        }

        public void OnInitialized(FlicManager manager)
        {
            FlicButton button = manager.CompleteGrabButton(RequestCode, (int)ResultCode, Data);
            if (button != null)
            {
                button.RegisterListenForBroadcast(FlicBroadcastReceiverFlags.UpOrDown | FlicBroadcastReceiverFlags.Removed);
                Toast.MakeText(Application.Context, "Grabbed a button", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(Application.Context, "Did not grab any button", ToastLength.Short).Show();
            }
        }
    }
}