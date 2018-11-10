using BikeHotel.GiantLeap;
using BikeHotel.GiantLeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BikeHotel.Onboarding
{
    [Preserve(AllMembers = true)]
    public class OnboardingViewModel : ViewModelBase
    {
        private IGiantLeapApiService ApiService = DependencyService.Get<IGiantLeapApiService>();
        private UserService UserService = DependencyService.Get<UserService>();

        private OnboardingStep currentOnboardingStep;
        public OnboardingStep CurrentOnboardingStep
        {
            get { return currentOnboardingStep; }
            set
            {
                if (value == currentOnboardingStep)
                    return;

                currentOnboardingStep = value;
                OnPropertyChanged(nameof(CurrentOnboardingStep));
            }
        }


        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value == isLoading)
                    return;

                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value == phoneNumber)
                    return;

                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(IsValidPhoneNumber));
            }
        }

        public bool IsValidPhoneNumber
        {
            get
            {
                // Eight digits
                return Regex.IsMatch(PhoneNumber ?? "", "\\b\\d{8}\\b");
            }
        }


        private string verificationCode;
        public string VerificationCode
        {
            get { return verificationCode; }
            set
            {
                if (value == verificationCode)
                    return;

                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
                OnPropertyChanged(nameof(IsValidVerificationCode));
            }
        }

        public bool IsValidVerificationCode
        {
            get
            {
                // Four digits
                return Regex.IsMatch(VerificationCode ?? "", "\\b\\d{4}\\b");
            }
        }


        private IList<Permit> availablePermits;
        public IList<Permit> AvailablePermits
        {
            get { return availablePermits; }
            set
            {
                if (value == availablePermits)
                    return;

                availablePermits = value;
                OnPropertyChanged(nameof(AvailablePermits));
            }
        }


        private Permit defaultPermit;
        public Permit DefaultPermit
        {
            get { return defaultPermit; }
            set
            {
                if (value == defaultPermit)
                    return;

                defaultPermit = value;
                OnPropertyChanged(nameof(DefaultPermit));
            }
        }

        public ICommand RequestVerificatonCodeCommand { get; private set; }
        public ICommand VerifyCodeCommand { get; private set; }
        public ICommand PreviousStepCommand { get; private set; }
        public ICommand FinishOnboardingCommend { get; private set; }
        public ICommand SelectDefaultPermitCommand { get; private set; }


        public OnboardingViewModel()
        {
            RequestVerificatonCodeCommand = new Command(RequestVerificationCode);
            PreviousStepCommand = new Command(PreviousStep);
            VerifyCodeCommand = new Command(VerifyCode);
            FinishOnboardingCommend = new Command(FinishOnboarding);
            SelectDefaultPermitCommand = new Command(SelectDefaultPermit);

            // Fetch phone number from storage
            PhoneNumber = UserService.PhoneNumber;
            CurrentOnboardingStep = OnboardingStep.PhoneNumber;
        }

        private void SelectDefaultPermit(object obj)
        {
            IsLoading = true;
            UserService.DefaultPermitId = DefaultPermit.Id;
            CurrentOnboardingStep = OnboardingStep.Done;
            IsLoading = false;
        }

        private void FinishOnboarding(object obj)
        {
            ((App)App.Current).SetMainPage();
        }

        private void PreviousStep(object obj)
        {
            IsLoading = true;
            switch (CurrentOnboardingStep)
            {
                case OnboardingStep.VerificationCode:
                    CurrentOnboardingStep = OnboardingStep.PhoneNumber;
                    break;
                case OnboardingStep.PhoneNumber:
                case OnboardingStep.Done:
                case OnboardingStep.Error:
                    break;
            }
            IsLoading = false;
        }

        private async void VerifyCode(object obj)
        {
            IsLoading = true;
            var result = await ApiService.RequestAccessTokenFromVerificationCodeAsync(PhoneNumber, VerificationCode);
            if (result != null)
            {
                await UserService.SetCachedAccessTokenAsync(result.AccessToken);
                await UserService.SetCachedRefreshTokenAsync(result.RefreshToken);
                UserService.UserId = result.UserId;

                // Load permits for next step
                AvailablePermits = await ApiService.GetMyPermitsAsync(PhoneNumber, result.AccessToken);
                UserService.Permits = AvailablePermits;
                DefaultPermit = AvailablePermits.FirstOrDefault();
                CurrentOnboardingStep = OnboardingStep.SelectPermit;
            }
            // TODO: Add error handling
            IsLoading = false;
        }

        private async void RequestVerificationCode(object obj)
        {
            IsLoading = true;
            UserService.PhoneNumber = PhoneNumber;
            var result = await ApiService.RequestVerificationCodeAsync(PhoneNumber);
            if (result)
                CurrentOnboardingStep = OnboardingStep.VerificationCode;

            // TODO: Add error handling
            // TODO: Read sms if permission is set to avoid having to enter verification code manually
            IsLoading = false;
        }
    }
}
