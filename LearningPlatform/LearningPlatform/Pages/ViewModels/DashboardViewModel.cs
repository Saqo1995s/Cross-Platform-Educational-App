using System.Collections.ObjectModel;
using IUniversity.Common.Models;
using LearningPlatform.Pages.ViewModels.Base;
using LearningPlatform.Utilities;

namespace LearningPlatform.Pages.ViewModels
{
    class DashboardViewModel : ViewModelBase
    {
        private ObservableCollection<Course> _courseList;
        private ObservableCollection<Course> _recentCourseList;
        private Course _selectedCourse;
        private UserInfo _userInfo;
        private string _group;

        public UserInfo UserInfo
        {
            get => _userInfo;
            set
            {
                _userInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Course> CourseList
        {
            get => _courseList;
            set
            {
                _courseList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Course> RecentCourseList
        {
            get => _recentCourseList;
            set
            {
                _recentCourseList = value;
                OnPropertyChanged();
            }
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                if (!RecentCourseList.Contains(_selectedCourse))
                    RecentCourseList.Add(_selectedCourse);
            }
        }

        public string Group => Settings.Group;

        public void FirePropertyChangedByName(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}
