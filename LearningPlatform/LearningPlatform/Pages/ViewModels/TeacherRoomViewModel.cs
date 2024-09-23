using System.Collections.Generic;
using IUniversity.Common.Models;
using LearningPlatform.Pages.ViewModels.Base;

namespace LearningPlatform.Pages.ViewModels
{
    public class TeacherRoomViewModel : ViewModelBase
    {
        private List<string> _groups;
        private List<Student> _students;
        private string _selectedGroup;

        public List<string> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public string SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();
            }
        }

        public List<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

    }
}
