using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using wpchttr.Model;
using wpchttr.View;
using wpchttr.Core;
using System.Collections.Generic;

namespace wpchttr.ViewModel
{
    public class MainViewModel : ViewModelBase
    { 
        private FrameworkElement contentControlView;

        public MainViewModel()
        {
            Messenger.Default.Register<SwitchViewMessage>(this,
                switchViewMessage => { SwitchView(switchViewMessage.ViewName); });
            ContentControlView = new SignIn();
            SignInCommand = new RelayCommand(CurrentUserSignIn, CurrentUserCanSignIn);

            // Debugging quick sign in
            CurrentUserSignIn();
        }

        public CurrentUser CurrentUser { get; set; }

        public string MSG { get; set; }

        public RelayCommand SignInCommand { get; private set; }

        public ICommand ChangeSignInViewCommand
        {
            get { return new RelayCommand(() => { SwitchView("SignInView"); }); }
        }

        public RelayCommand RefreshProfileCommand { get; private set; }
        private bool RefreshingProfile { get; set; }

        public FrameworkElement ContentControlView
        {
            get { return contentControlView; }
            set
            {
                contentControlView = value;
                RaisePropertyChanged("ContentControlView");
            }
        }

        public void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "SignInView":
                    ContentControlView = new SignIn();
                    ContentControlView.DataContext = this;
                    break;
                case "PivotView":
                    ContentControlView = new chttrPivot();
                    ContentControlView.DataContext = this;
                    break;
            }
        }

        private async void CurrentUserSignIn()
        {
            CurrentUser = new CurrentUser("fultonm@wartimestudios.com", "mike2009"); // debug
            if (CurrentUser.SignIn())
            {
                CurrentUser.Relationships = new Relationships();
                CurrentUser.Chats = new Chats(CurrentUser.Id);
                SwitchView("PivotView");
            }
            else
            {
                await ShowDialog("Sign in failed. Check internet connection or email and password?");
            }

            //foreach (var s in CurrentUser.ErrorInformation)
            //{
            //    MSG += s;
            //}
            //foreach (var s in CurrentUser.Relationships.ErrorInformation)
            //{
            //    MSG += s;
            //}
        }

        private bool CurrentUserCanSignIn()
        {
            return true;
        }

        private void RefreshProfile()
        {
            RefreshingProfile = true;
            CurrentUser.Relationships = new Relationships();
            List<Chat> chats = CurrentUser.Chats.ChatCollection;
            RefreshingProfile = false;
        }

        private bool CanRefreshProfile()
        {
            return !RefreshingProfile;
        }

        private async Task ShowDialog(string message)
        {
            var md = new MessageDialog(message);
            await md.ShowAsync();
        }
    }
}