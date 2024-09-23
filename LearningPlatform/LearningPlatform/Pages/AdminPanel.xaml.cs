using IUniversity.Common.Models;
using IUniversity.Common.Models.Requests;
using LearningPlatform.Pages.ViewModels;
using LearningPlatform.Wrappers;
using System;
using System.Threading.Tasks;
using LearningPlatform.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningPlatform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPanel : ContentPage
    {
        public UserInfo AdminInfo { get; set; }

        private LearningPlatformRestApiClient RestClient { get; set; } = LearningPlatformRestApiClient.Instance;

        private AdminPanelViewModel ViewModel { get; set; }

        public AdminPanel()
        {
            InitializeComponent();

            ViewModel = BindingContext as AdminPanelViewModel;

            Task.Factory.StartNew(async () =>
            {
                var refreshed = await TryRefreshExpiredToken();

                if (!refreshed) //Otherwise application should be closed ...
                {
                    await FillCourseTeachersPicker();
                    await FillCoursesPicker();
                }
            });
        }

        public AdminPanel(UserInfo userInfo) : this()
        {
            AdminInfo = userInfo;
        }

        private async Task FillCourseTeachersPicker() //Dropdown
        {
            ViewModel.Teachers = await RestClient.GetTeachersAsync();
        }

        private async Task FillCoursesPicker()
        {
            ViewModel.Courses = await RestClient.GetCoursesAsync() ?? new Course[] { };
        }

        private async void studentSubmitBtn_OnClicked(object sender, EventArgs e)
        {
            var registrationRequest = new RegisterRequest
            {
                Email = studentEmail.Text,
                Password = studentPassword.Text,
                ConfirmPassword = studentConfirmPassword.Text,
                FirstName = studentFirstName.Text,
                LastName = studentLastName.Text,
                Group = StudentGroupsPicker.SelectedItem.ToString(),
                Username = studentEmail.Text,
                Role = "student"
            };

            _ = await RestClient.RegisterNewUserAsync(registrationRequest, UserTypes.Student);
        }

        private async void teacherSubmitBtn_Clicked(object sender, EventArgs e)
        {
            var registrationRequest = new RegisterRequest
            {
                Email = teacherEmail.Text,
                Password = teacherPassword.Text,
                ConfirmPassword = teacherConfirmPassword.Text,
                FirstName = teacherFirstName.Text,
                LastName = teacherLastName.Text,
                Username = teacherEmail.Text,
                Role = "teacher"
            };

            _ = await RestClient.RegisterNewUserAsync(registrationRequest, UserTypes.Teacher);
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

        private async Task<bool> TryRefreshExpiredToken()
        {
            var refreshed = await RestClient.RefreshTokenAsync();

            if (refreshed)
            {
                await DisplayAlert("Session was expired",
                    "Your session was expired,\nLog in again to continue use application!", "Ok");

                CleanupSettings();
            }

            return refreshed;
        }

        private async void CourseSubmitBtn_OnClicked(object sender, EventArgs e)
        {
            var teacher = (Teacher)courseTeacher.SelectedItem;

            var course = new Course
            {
                Name = courseTitle.Text,
                Content = courseContent.Text,
                TeacherId = teacher.Id,
                ShortDescription = courseShortDescr.Text
            };

            var result = await RestClient.AddNewCourseAsync(course);

            if (result) await FillCoursesPicker();
        }

        private async void AssignSubmitBtn_OnClicked(object sender, EventArgs e)
        {
            var courseAssignment = new CourseAssignment
            {
                CourseId = ((Course)coursesPicker.SelectedItem).Id,
                Group = GroupsPicker.SelectedItem.ToString()
            };

            var result = await RestClient.AssignCourseToGroupAsync(courseAssignment);
        }
    }
}