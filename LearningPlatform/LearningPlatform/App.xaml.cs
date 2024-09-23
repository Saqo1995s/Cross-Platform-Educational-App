using LearningPlatform.Pages;
using Xamarin.Forms;

namespace LearningPlatform
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var isUserLoggedIn = false; //TODO: change with logic

            var navigationPage = new NavigationPage(isUserLoggedIn 
                ? (Page) new HomePage() 
                : new LoginPage());

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
