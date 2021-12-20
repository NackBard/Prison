using Prison.Core;
using System.Linq;
using System.Windows;

namespace Prison.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public object[] views = new object[]
        {
            new AccessLevelViewModel(),
            new AccountingDiningVisitViewModel(),
            new AccountingPrisonerViewModel(),
            new AccountingRehabilitationWorkViewModel(),
            new AccountingTypeViewModel(),
            new BehaviorAssessmentViewModel(),
            new DishViewModel(),
            new GenderViewModel(),
            new JournalArrivalAndDepartureViewModel(),
            new PostViewModel(),
            new PrisonerViewModel(),
            new ProductViewModel(),
            new ProsecutionViewModel(),
            new SalesAccountingViewModel(),
            new SetViewModel(),
            new StatusViewModel(),
            new TypeProductViewModel(),
            new WarehouseViewModel(),
            new WorkerViewModel(),
            new WorkViewModel()
        };

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
            CurrentView = views.First();
            NextViewCommand = new RelayCommand(o => NextView(), param => CanNext);
            PreviousViewCommand = new RelayCommand(o => PreviousView(), param => CanPrevious);
        }

    }
}
