using System.Collections.Generic;
using LearningPlatform.Pages.ViewModels.Base;

namespace LearningPlatform.Pages.ViewModels
{
    class AgendaViewModel : ViewModelBase
    {
        private List<string> _eventList;

        public List<string> EventList
        {
            get => _eventList;
            set
            {
                _eventList = value;
                OnPropertyChanged();
            }
        }
    }
}
