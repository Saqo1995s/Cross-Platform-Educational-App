using System;
using System.Threading.Tasks;
using IUniversity.Common.Models;
using LearningPlatform.Extensions;
using LearningPlatform.Pages.ViewModels;
using LearningPlatform.Utilities;
using LearningPlatform.Wrappers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningPlatform.Pages.HomePageTabs.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        private CourseViewModel ViewModel { get; }

        private Course Course { get; }

        private LearningPlatformRestApiClient RestClient { get; set; } = LearningPlatformRestApiClient.Instance;

        public CoursePage(Course course) : this()
        {
            ViewModel = (CourseViewModel)this.BindingContext;

            Course = course;

            //Async context
            Task.Factory.StartNew(async () =>
            {
                Course.Participants++;
                await RestClient.UpdateCourseAsync(Course);

                ViewModel.TeacherName = string.IsNullOrWhiteSpace(Course.CourseTeacher.FullName) 
                    ? string.Empty 
                    : Course.CourseTeacher.FullName;
            });

            ViewModel.CourseTitle = course.Name;
            ViewModel.Participants = course.Participants;

            if (course.Content.IsUrl())
            {
                courseContentWebView.Source = course.Content;
            }
            else
            {
                HtmlWebViewSource source = new HtmlWebViewSource { Html = course.Content };
                courseContentWebView.Source = source;
            }
        }

        public CoursePage()
        {
            InitializeComponent();
        }

        private async void DoneBtn_OnClicked(object sender, EventArgs e)
        {
            using var dialog = await UserDialogs.ShowLoadingAsync();

            Course.CoursePasses++;
            Course.Participants--;

            await RestClient.UpdateCourseAsync(Course);

            var teacherAssignment = new TeacherAssignment
            {
                CourseId = Course.Id,
                Group = Settings.Group,
                StudentId = Settings.UserId,
                TeacherId = Course.TeacherId
            };

            await RestClient.AddTeacherAssignment(teacherAssignment);

            await Navigation.PopAsync();
        }
    }
}