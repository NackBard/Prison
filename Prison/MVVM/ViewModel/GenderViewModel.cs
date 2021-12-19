using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class GenderViewModel : TableViewModel, ICRUD, ITableModel
    {
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

        private ObservableCollection<Gender> _gendersDelete;
        public ObservableCollection<Gender> GendersDelete
        {
            get => _gendersDelete;
            set
            {
                _gendersDelete = value;
                OnPropertyChanged();
            }
        }

        private Gender _genderDeleteSelected;
        public Gender GenderDeleteSelected
        {
            get => _genderDeleteSelected;
            set
            {
                _genderDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Gender _genderSelected;
        public Gender GenderSelected
        {
            get => _genderSelected;
            set
            {
                _genderSelected = value;
                GenderForEdit = (Gender)_genderSelected?.Clone() ?? new Gender();
                OnPropertyChanged();
            }
        }
        private Gender _genderForEdit;
        public Gender GenderForEdit
        {
            get => _genderForEdit;
            set
            {
                _genderForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => GenderSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => GenderSelected != null;
        public bool CanClear => GendersDelete.Count > 0;
        public bool CanRecover => GenderDeleteSelected != null;

        public GenderViewModel()
        {
            GenderForEdit = new Gender();
            Genders = new ObservableCollection<Gender>();
            GendersDelete = new ObservableCollection<Gender>();
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
            Genders.Add(GenderDeleteSelected);
            GendersDelete.Remove(GenderDeleteSelected);
        }
        public void Drop()
        {
            GendersDelete.Add(GenderSelected);
            Genders.Remove(GenderSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                GenderForEdit.Id = null;
                await DataSender.PostRequest(nameof(Genders), GenderForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in GendersDelete)
            {
                await DataSender.DeleteRequest(nameof(Genders), item.Id.Value);
            }
            GendersDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Genders = await ApiConnector.GetAll<Gender>(nameof(Genders));
            foreach (var item in GendersDelete)
                Genders.Remove(Genders.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Genders), GenderSelected.Id.Value, GenderForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(GenderForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Genders), GenderSelected.Id.Value, GenderForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Genders, nameof(Genders));
        }
    }
}

