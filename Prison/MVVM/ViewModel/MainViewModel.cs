using Prison.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Prison.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public object[] views;

        #region NextCommand
        private RelayCommand _nextViewCommand;
        public RelayCommand NextViewCommand
        {
            get => _nextViewCommand;
            set
            {
                _nextViewCommand = value;
                OnPropertyChanged(); 
            }
        }
        public void NextView()
        {
            currentViewId++;
            CurrentView = views[currentViewId];
        }
        private bool CanNext => views.Length - 1 > currentViewId;
        #endregion NextCommand

        #region PreviousCommand
        private RelayCommand _previousViewCommand;
        public RelayCommand PreviousViewCommand
        {
            get => _previousViewCommand;
            set
            {
                _previousViewCommand = value;
                OnPropertyChanged();
            }
        }
        public void PreviousView()
        {
            currentViewId--;
            CurrentView = views[currentViewId];
        }
        private bool CanPrevious => currentViewId > 0;
        #endregion PreviousCommand

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

        private int currentViewId;

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
            views = new object[] { new AccessLevelViewModel(), new PostViewModel() };
            CurrentView = views.First();
            NextViewCommand = new RelayCommand(o => NextView(), param => CanNext);
            PreviousViewCommand = new RelayCommand(o => PreviousView(), param => CanPrevious);
        }

    }
}
