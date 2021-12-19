using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class StatusViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Status> _statuses;
        public ObservableCollection<Status> Statuses
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Status> _statusesDelete;
        public ObservableCollection<Status> StatusesDelete
        {
            get => _statusesDelete;
            set
            {
                _statusesDelete = value;
                OnPropertyChanged();
            }
        }

        private Status _statusDeleteSelected;
        public Status StatusDeleteSelected
        {
            get => _statusDeleteSelected;
            set
            {
                _statusDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Status _statusSelected;
        public Status StatusSelected
        {
            get => _statusSelected;
            set
            {
                _statusSelected = value;
                StatusForEdit = (Status)_statusSelected?.Clone() ?? new Status();
                OnPropertyChanged();
            }
        }
        private Status _statusForEdit;
        public Status StatusForEdit
        {
            get => _statusForEdit;
            set
            {
                _statusForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => StatusSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => StatusSelected != null;
        public bool CanClear => StatusesDelete.Count > 0;
        public bool CanRecover => StatusDeleteSelected != null;

        public StatusViewModel()
        {
            StatusForEdit = new Status();
            Statuses = new ObservableCollection<Status>();
            StatusesDelete = new ObservableCollection<Status>();
            Init();
        }
        public void Init()
        {
            ReadAsync();
            DeleteCommand = new RelayCommand(o => Drop(), param => CanDelete);
            CreateCommand = new RelayCommand(o => CreateAsync());
            UpdateCommand = new RelayCommand(o => UpdateWithReadAsync(), param => CanUpdate);
            RecoverCommand = new RelayCommand(o => Recover(), param => CanRecover);
            ClearCommand = new RelayCommand(o => DeleteAsync(), param => CanClear);
            ExportCommand = new RelayCommand(o => Export());
        }
        public void Recover()
        {
            Statuses.Add(StatusDeleteSelected);
            StatusesDelete.Remove(StatusDeleteSelected);
        }
        public void Drop()
        {
            StatusesDelete.Add(StatusSelected);
            Statuses.Remove(StatusSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                StatusForEdit.Id = null;
                await DataSender.PostRequest(nameof(Status), StatusForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in StatusesDelete)
            {
                await DataSender.DeleteRequest(nameof(Status), item.Id.Value);
            }
            StatusesDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Statuses = await ApiConnector.GetAll<Status>(nameof(Status));
            foreach (var item in StatusesDelete)
                Statuses.Remove(Statuses.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Status), StatusSelected.Id.Value, StatusForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(StatusForEdit.Name);
        }

        public  async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Status), StatusSelected.Id.Value, StatusForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Statuses, nameof(Statuses));
        }
    }
}
