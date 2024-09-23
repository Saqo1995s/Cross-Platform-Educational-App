using LearningPlatform.Pages.ViewModels.Base;

namespace LearningPlatform.Pages.ViewModels
{
    class CourseViewModel : ViewModelBase
    {
        private string _courseTitle;
        private string _teacherName;
        private int _participants;

        public string CourseTitle
        {
            get => _courseTitle;
            set
            {
                _courseTitle = value;
                OnPropertyChanged();
            }
        }

        public string TeacherName
        {
            get => _teacherName;
            set
            {
                _teacherName = value;
                OnPropertyChanged();
            }
        }

        public int Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                OnPropertyChanged();
            }
        }
    }
}
