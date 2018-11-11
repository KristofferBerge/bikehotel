using BikeHotel.Flic;
using BikeHotel.GiantLeap;
using BikeHotel.GiantLeap.Mock;
using BikeHotel.Onboarding;
using BikeHotel.Unlock;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BikeHotel
{
    public partial class App : Application
    {
        public App()
        {
            SetupDependencies();
            InitializeComponent();
            SetMainPage();

        }

        public void SetMainPage()
        {
            if (DependencyService.Get<UserService>().IsUserConfigured)
                MainPage = new NavigationPage(new UnlockPage());
            else
                MainPage = new NavigationPage(new OnboardingPage());
        }

        private void SetupDependencies()
        {
            DependencyService.Register<UserService>();
            DependencyService.Register<GiantLeapApiService>();
            //DependencyService.Register<GiantLeapMockApiService>();
        }

        protected override void OnStart()
        {
            AppCenter.Start("2454f05b-14ca-43fd-bff6-87f15b3129ac",
       typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
