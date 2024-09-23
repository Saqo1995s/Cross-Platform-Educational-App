using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IUniversity.Common.Models;
using LearningPlatform.Pages.HomePageTabs.DashboardPages;
using LearningPlatform.Pages.ViewModels;
using LearningPlatform.Utilities;
using LearningPlatform.Wrappers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LearningPlatform.Pages.HomePageTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentView
    {
        private DashboardViewModel ViewModel { get; set; }

        private LearningPlatformRestApiClient RestClient { get; set; } = LearningPlatformRestApiClient.Instance;

        public DashboardPage()
        {
            InitializeComponent();

            ViewModel = (DashboardViewModel)this.BindingContext;

            ViewModel.RecentCourseList = new ObservableCollection<Course>();

            MessagingCenter.Subscribe<HomePage, UserInfo>(this, "userInfo", UpdateUserInfoAndGetCoursesForUser);
        }

        private void UpdateUserInfoAndGetCoursesForUser(HomePage sender, UserInfo info)
        {
            ViewModel.UserInfo = info;

            Task.Factory.StartNew(async () =>
            {
                var group = ((Student)ViewModel.UserInfo.User).Group;
                var courses = await RestClient.GetCoursesForGroup(group);

                ViewModel.CourseList = new ObservableCollection<Course>(courses);
            });
        }

        private async void CoursesCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using var dialog = await UserDialogs.ShowLoadingAsync();

            var course = ViewModel.SelectedCourse;

            var teacher = await RestClient.GetTeacherByIdAsync(course.TeacherId);
            course.CourseTeacher = teacher;

            await Navigation.PushAsync(new CoursePage(course));
        }
    }
}