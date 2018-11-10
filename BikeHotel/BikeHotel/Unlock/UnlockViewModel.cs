using BikeHotel.GiantLeap;
using BikeHotel.GiantLeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BikeHotel.Unlock
{
    [Preserve(AllMembers = true)]
    public class UnlockViewModel : ViewModelBase
    {
        private UserService UserService = DependencyService.Get<UserService>();
        private IGiantLeapApiService ApiService = DependencyService.Get<IGiantLeapApiService>();

        private Permit selectedPermit;
        public Permit SelectedPermit
        {
            get { return selectedPermit; }
            set
            {
                if (value == selectedPermit)
                    return;

                selectedPermit = value;
                OnPropertyChanged(nameof(SelectedPermit));
            }
        }

        public ICommand UnlockCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }

        public UnlockViewModel()
        {
            SelectedPermit = UserService.Permits.Where(x => x.Id == UserService.DefaultPermitId).FirstOrDefault();
            UnlockCommand = new Command(Unlock, CanUnlock);
            OpenSettingsCommand = new Command(OpenSettings);
        }

        private void OpenSettings(object obj)
        {
            ((App)App.Current).OpenSettings();
        }

        private bool canUnlock = true;
        private bool CanUnlock(object arg)
        {
            return canUnlock;
        }

        private async void Unlock(object obj)
        {
            canUnlock = false;
            ((Command)UnlockCommand).ChangeCanExecute();
            try
            {
                await UnlockService.TryUnlock();
            }
            catch (Exception e)
            {
                // TODO: error handling
                throw;
            }
            finally
            {
                canUnlock = true;
                ((Command)UnlockCommand).ChangeCanExecute();
            }
        }
    }
}
