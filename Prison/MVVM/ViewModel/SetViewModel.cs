using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class SetViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Set> _sets;
        public ObservableCollection<Set> Sets
        {
            get => _sets;
            set
            {
                _sets = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Set> _setsDelete;
        public ObservableCollection<Set> SetsDelete
        {
            get => _setsDelete;
            set
            {
                _setsDelete = value;
                OnPropertyChanged();
            }
        }

        private Set _setDeleteSelected;
        public Set SetDeleteSelected
        {
            get => _setDeleteSelected;
            set
            {
                _setDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Set _setSelected;
        public Set SetSelected
        {
            get => _setSelected;
            set
            {
                _setSelected = value;
                SetForEdit = (Set)_setSelected?.Clone() ?? new Set();
                OnPropertyChanged();
            }
        }
        private Set _setForEdit;
        public Set SetForEdit
        {
            get => _setForEdit;
            set
            {
                _setForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => SetSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => SetSelected != null;
        public bool CanClear => SetsDelete.Count > 0;
        public bool CanRecover => SetDeleteSelected != null;

        public SetViewModel()
        {
            SetForEdit = new Set();
            Sets = new ObservableCollection<Set>();
            SetsDelete = new ObservableCollection<Set>();
            Init();
        }
        public void Init()
        {
            ReadAsync();
            DeleteCommand = new RelayCommand(o => Drop(), param => CanDelete);
            CreateCommand = new RelayCommand(o => CreateAsync());
            UpdateCommand = new RelayCommand(o => UpdateAsync(), param => CanUpdate);
            RecoverCommand = new RelayCommand(o => Recover(), param => CanRecover);
            ClearCommand = new RelayCommand(o => DeleteAsync(), param => CanClear);
            ExportCommand = new RelayCommand(o => Export());
        }
        public void Recover()
        {
            Sets.Add(SetDeleteSelected);
            SetsDelete.Remove(SetDeleteSelected);
        }
        public void Drop()
        {
            SetsDelete.Add(SetSelected);
            Sets.Remove(SetSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                SetForEdit.Id = null;
                await DataSender.PostRequest(nameof(Sets), SetForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in SetsDelete)
                await DataSender.DeleteRequest(nameof(Sets), item.Id.Value);
            SetsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Sets = await ApiConnector.GetAll<Set>(nameof(Sets));
            foreach (var item in SetsDelete)
                Sets.Remove(Sets.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Sets), SetSelected.Id.Value, SetForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(SetForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Sets), SetSelected.Id.Value, SetForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Sets, nameof(Sets));
        }
    }
}
