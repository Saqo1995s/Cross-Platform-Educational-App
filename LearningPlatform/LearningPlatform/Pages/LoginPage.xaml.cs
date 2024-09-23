using System;
using System.Net.Http;
using System.Threading.Tasks;
using IUniversity.Common.Models;
using LearningPlatform.Utilities;
using LearningPlatform.Wrappers;
using IUniversity.Common.Constants;
using Xamarin.Forms;
using XamUIDemo.Animations;

namespace LearningPlatform.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                await ViewAnimations.FadeAnimY(Logo);
                await ViewAnimations.FadeAnimY(UsernamePancake);
                await ViewAnimations.FadeAnimY(PassPancake);
                await ViewAnimations.FadeAnimY(LoginButton);

                //await CheckServiceAndShowDialogTask();
            });
        }

        private async Task CheckServiceAndShowDialogTask()
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var dialog = await UserDialogs.ShowLoadingAsync("Checking Service"))
            {
                var isAvailable = await IsServiceApiAvailable();

                dialog.Hide();

                if (!isAvailable) UserDialogs.ShowToast("Service down,\nPlease try again later!", ToastDurationConstants.Long);
            }
        }

        public async Task<bool> IsServiceApiAvailable()
        {
            var restClient = LearningPlatformRestApiClient.Instance;

            var result = await restClient.CheckApiServiceAvailable();

            return result;
        }

        protected async void Login(object s, EventArgs e)
        {
            //not empty validation
            if (!string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                var restClient = LearningPlatformRestApiClient.Instance;

                try
                {
                    using var dialog = await UserDialogs.ShowLoadingAsync();

                    var authResult = await restClient.LoginUser(UsernameEntry.Text, PasswordEntry.Text);

                    if (authResult.Success)
                    {
                        restClient.AddBearerAuthorization(authResult.Token); //Adding bearer token for an individual user

                        Settings.BearerToken = authResult.Token;
                        Settings.RefreshToken = authResult.RefreshToken;
                        //Settings.IsUserLoggedIn = true;

                        switch (authResult.Role)
                        {
                            case "admin":
                                var admin = await restClient.GetAdminByAccountId(authResult.UserId);

                                var adminInfo = new UserInfo
                                {
                                    DisplayName = admin.FullName,
                                    Email = admin.Email,
                                    User = admin
                                };

                                await Navigation.PushAsync(new AdminPanel(adminInfo));

                                UsernameEntry.Text = string.Empty;
                                PasswordEntry.Text = string.Empty;

                                break;
                            case "student":
                                var student = await restClient.GetStudentByAccountId(authResult.UserId);

                                var studentInfo = new UserInfo
                                {
                                    DisplayName = student.FullName,
                                    Email = student.Email,
                                    User = student
                                };

                                Settings.Group = student.Group;

                                await Navigation.PushAsync(new HomePage(studentInfo));

                                UsernameEntry.Text = string.Empty;
                                PasswordEntry.Text = string.Empty;
                                break;
                            case "teacher":
                                var teacher = await restClient.GetTeacherByAccountId(authResult.UserId);

                                var teacherInfo = new UserInfo
                                {
                                    DisplayName = teacher.FullName,
                                    Email = teacher.Email,
                                    User = teacher
                                };

                                await Navigation.PushAsync(new TeacherRoom(teacherInfo));

                                UsernameEntry.Text = string.Empty;
                                PasswordEntry.Text = string.Empty;
                                break;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    await DisplayAlert("Oops", "Invalid user credentials,\nPlease check credentials and try again!",
                        "Ok");
                }
            }
        }
    }
}