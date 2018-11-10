using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeHotel.Flic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlicSetupPage : ContentPage
    {
        public FlicSetupPage()
        {
            InitializeComponent();
        }

        private void AddFlicClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IFlicService>().GrabButton();
        }
    }
}