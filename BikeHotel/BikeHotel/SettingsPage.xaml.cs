using BikeHotel.Flic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeHotel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ConfigureFlicClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FlicSetupPage());
        }

        private async void SignOutClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Sign out", "Are you sure you want to sign out?", "Yes", "No"))
                await DependencyService.Get<UserService>().ClearAllDataAndSignOutAsync();
        }
    }
}