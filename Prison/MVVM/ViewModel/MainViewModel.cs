using Prison.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prison.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public List<object> views = new List<object>();

        private Visibility _authVisibility;
        public Visibility AuthVisibility
        {
            get => _authVisibility;
            set
            {
                _authVisibility = value;
                if (value == Visibility.Visible)
                    MainVisibility = Visibility.Collapsed;
                else
                    MainVisibility = Visibility.Visible;

                OnPropertyChanged();
            }
        }

        private Visibility _mainVisibility;
        public Visibility MainVisibility
        {
            get => _mainVisibility;
            set
            {
                _mainVisibility = value;
                OnPropertyChanged();
            }
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            views = new List<object>();
            views.Add(new AccessLevelViewModel());
            CurrentView = views.First();
        }
    }
}
