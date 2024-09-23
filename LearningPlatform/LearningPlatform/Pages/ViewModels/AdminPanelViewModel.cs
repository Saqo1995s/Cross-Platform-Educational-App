using IUniversity.Common.Models;
using LearningPlatform.Pages.ViewModels.Base;

namespace LearningPlatform.Pages.ViewModels
{
    class AdminPanelViewModel : ViewModelBase
    {
        private Teacher[] _teachers;
        private Course[] _courses;

        public Teacher[] Teachers
        {
            get => _teachers;
            set
            {
                _teachers = value;
                OnPropertyChanged();
            }
        }

        public Course[] Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }

        public string[] Groups => new[] {"Group 1", "Group 2", "Group 3", "Group 4", "Group 5", "Group 6"};
    }
}
