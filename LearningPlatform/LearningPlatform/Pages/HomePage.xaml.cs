using System;
using IUniversity.Common.Models;
using LearningPlatform.Utilities;
using LearningPlatform.Wrappers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningPlatform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private LearningPlatformRestApiClient RestClient { get; set; } = LearningPlatformRestApiClient.Instance;

        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(UserInfo info) : this()
        {
            MessagingCenter.Send(this, "userInfo", info);
        }

        private async void LogoutBtn_OnClicked(object sender, EventArgs e)
        {
            await RestClient.LogoutUserAsync();

            CleanupSettings();

            await Navigation.PopToRootAsync(true);
        }

        private void CleanupSettings()
        {
            //Settings.IsUserLoggedIn = false;
            Settings.BearerToken = string.Empty;
            Settings.RefreshToken = string.Empty;
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<HomePage>(this, "userInfo");

            base.OnDisappearing();
        }
    }
}