using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeHotel.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            InitializeComponent();
            ((OnboardingViewModel)BindingContext).PropertyChanged += OnSomethingChanged;
        }

        private async void OnSomethingChanged(object sender, PropertyChangedEventArgs e)
        {
            OnboardingViewModel vm = (OnboardingViewModel)BindingContext;
            if (e.PropertyName == nameof(vm.IsLoading))
            {
                if (vm.IsLoading)
                {
                    LoadingOverlay.IsVisible = true;
                    await LoadingOverlay.FadeTo(1);
                    LoadingSpinner.IsVisible = true;
                }
                else
                {
                    LoadingSpinner.IsVisible = false;
                    await LoadingOverlay.FadeTo(0);
                    LoadingOverlay.IsVisible = false;
                }
            }
        }
    }
}