using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class WorkerViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Worker> _workers;
        public ObservableCollection<Worker> Workers
        {
            get => _workers;
            set
            {
                _workers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Worker> _workersDelete;
        public ObservableCollection<Worker> WorkersDelete
        {
            get => _workersDelete;
            set
            {
                _workersDelete = value;
                OnPropertyChanged();
            }
        }

        private Worker _workerDeleteSelected;
        public Worker WorkerDeleteSelected
        {
            get => _workerDeleteSelected;
            set
            {
                _workerDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Worker _workerSelected;
        public Worker WorkerSelected
        {
            get => _workerSelected;
            set
            {
                _workerSelected = value;
                WorkerForEdit = (Worker)_workerSelected?.Clone() ?? new Worker();
                OnPropertyChanged();
            }
        }
        private Worker _workerForEdit;
        public Worker WorkerForEdit
        {
            get => _workerForEdit;
            set
            {
                _workerForEdit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Gender> _genders;
        public ObservableCollection<Gender> Genders
        {
            get => _genders;
            set
            {
                _genders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => WorkerSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => WorkerSelected != null;
        public bool CanClear => WorkersDelete.Count > 0;
        public bool CanRecover => WorkerDeleteSelected != null;

        public WorkerViewModel()
        {
            WorkerForEdit = new Worker();
            Workers = new ObservableCollection<Worker>();
            WorkersDelete = new ObservableCollection<Worker>();
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
            Workers.Add(WorkerDeleteSelected);
            WorkersDelete.Remove(WorkerDeleteSelected);
        }
        public void Drop()
        {
            WorkersDelete.Add(WorkerSelected);
            Workers.Remove(WorkerSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                WorkerForEdit.Id = null;
                await DataSender.PostRequest(nameof(Workers), WorkerForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in WorkersDelete)
                await DataSender.DeleteRequest(nameof(Workers), item.Id.Value);
            WorkersDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Posts = await ApiConnector.GetAll<Post>(nameof(Posts));
            Genders = await ApiConnector.GetAll<Gender>(nameof(Genders));
            Workers = await ApiConnector.GetAll<Worker>(nameof(Workers));
            foreach (var item in WorkersDelete)
                Workers.Remove(Workers.First(level => level.Id == item.Id));
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Workers), WorkerSelected.Id.Value, WorkerForEdit);
        }

        public bool Validate()
        {
            return true;
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Workers), WorkerSelected.Id.Value, WorkerForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Workers, nameof(Workers));
        }
    }
}

