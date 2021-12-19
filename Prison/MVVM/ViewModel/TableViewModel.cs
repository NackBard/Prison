using Prison.Core;

namespace Prison.MVVM.ViewModel
{
    class TableViewModel : ObservableObject
    {
        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand
        {
            get => _updateCommand;
            set
            {
                _updateCommand = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get => _deleteCommand;
            set
            {
                _deleteCommand = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand _createCommand;
        public RelayCommand CreateCommand
        {
            get => _createCommand;
            set
            {
                _createCommand = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _clearCommand;
        public RelayCommand ClearCommand
        {
            get => _clearCommand;
            set
            {
                _clearCommand = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _recoverCommand;
        public RelayCommand RecoverCommand
        {
            get => _recoverCommand;
            set
            {
                _recoverCommand = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand _exportCommand;
        public RelayCommand ExportCommand
        {
            get => _exportCommand;
            set
            {
                _exportCommand = value;
                OnPropertyChanged();
            }
        }
    }
}
