using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUniversity.Common.Models;
using LearningPlatform.Pages.ViewModels;
using LearningPlatform.Wrappers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace LearningPlatform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeacherRoom : ContentPage
    {
        private LearningPlatformRestApiClient RestClient => LearningPlatformRestApiClient.Instance;

        private UserInfo UserInfo { get; }

        private TeacherRoomViewModel ViewModel { get; set; }

        private IGrouping<string, TeacherAssignment>[] GroupedAssignments { get; set; }

        public TeacherRoom(UserInfo info) : this()
        {
            UserInfo = info;
        }

        public TeacherRoom()
        {
            InitializeComponent();

            ViewModel = (TeacherRoomViewModel)this.BindingContext;

            ViewModel.Groups = new List<string>();
            ViewModel.Students = new List<Student>();

            Task.Factory.StartNew(async () =>
            {
                var teacherAssignments = await RestClient.GetTeacherAssignmentsByTeacherId(UserInfo.User.Id);

                GroupedAssignments = teacherAssignments.GroupBy(x => x.Group).ToArray();
                GroupedAssignments.ForEach(x => { ViewModel.Groups.Add(x.Key); }); //Filling Groups
            });
        }

        private async void Expander_OnTapped(object sender, EventArgs e)
        {
            var expander = (Expander)sender;

            //var assignmentsForGroup = GroupedAssignments
            //    .Where(x => x.Key.Equals());

            var courses = await RestClient.GetCoursesForGroup(ViewModel.SelectedGroup);

            var template = new DataTemplate(() =>
            {
                var label = new Label();
                label.SetBinding(Label.TextProperty, nameof(Course.Name));

                return new ViewCell { View = label};
            });

            expander.Content = new CollectionView
            {
                ItemTemplate = template,
                ItemsSource = courses
            };
        }
    }
}