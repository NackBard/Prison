using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;

namespace Prison.MVVM.ViewModel
{
    class WorkViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Work> _works;
        public ObservableCollection<Work> Works
        {
            get => _works;
            set
            {
                _works = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Work> _worksDelete;
        public ObservableCollection<Work> WorksDelete
        {
            get => _worksDelete;
            set
            {
                _worksDelete = value;
                OnPropertyChanged();
            }
        }

        private Work _workDeleteSelected;
        public Work WorkDeleteSelected
        {
            get => _workDeleteSelected;
            set
            {
                _workDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Work _workSelected;
        public Work WorkSelected
        {
            get => _workSelected;
            set
            {
                _workSelected = value;
                WorkForEdit = (Work)_workSelected?.Clone() ?? new Work();
                OnPropertyChanged();
            }
        }
        private Work _workForEdit;
        public Work WorkForEdit
        {
            get => _workForEdit;
            set
            {
                _workForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => WorkSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => WorkSelected != null;
        public bool CanClear => WorksDelete.Count > 0;
        public bool CanRecover => WorkDeleteSelected != null;

        public WorkViewModel()
        {
            WorkForEdit = new Work();
            Works = new ObservableCollection<Work>();
            WorksDelete = new ObservableCollection<Work>();
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
            WorkSelected = (Work)WorkDeleteSelected.Clone();
            WorkForEdit.IsDeleted = false;
            UpdateAsync();
            Works.Add(WorkDeleteSelected);
            WorksDelete.Remove(WorkDeleteSelected);
        }
        public void Drop()
        {
            WorkForEdit.IsDeleted = true;
            UpdateAsync();
            WorksDelete.Add(WorkSelected);
            Works.Remove(WorkSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                WorkForEdit.Id = null;
                await DataSender.PostRequest(nameof(Works), WorkForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in WorksDelete)
            {
                await DataSender.DeleteRequest(nameof(Works), item.Id.Value);
            }
            WorksDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            WorksDelete = new ObservableCollection<Work>();
            Works = new ObservableCollection<Work>();
            var all = await ApiConnector.GetAll<Work>(nameof(Works));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    WorksDelete.Add(item);
                else
                    Works.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Works), WorkSelected.Id.Value, WorkForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(WorkForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Works), WorkSelected.Id.Value, WorkForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Works, nameof(Works));
        }
    }
}
