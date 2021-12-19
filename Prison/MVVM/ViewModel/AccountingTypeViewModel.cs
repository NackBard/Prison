using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class AccountingTypeViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<AccountingType> _accountingTypes;
        public ObservableCollection<AccountingType> AccountingTypes
        {
            get => _accountingTypes;
            set
            {
                _accountingTypes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AccountingType> _accountingTypesDelete;
        public ObservableCollection<AccountingType> AccountingTypesDelete
        {
            get => _accountingTypesDelete;
            set
            {
                _accountingTypesDelete = value;
                OnPropertyChanged();
            }
        }

        private AccountingType _accountingTypeDeleteSelected;
        public AccountingType AccountingTypeDeleteSelected
        {
            get => _accountingTypeDeleteSelected;
            set
            {
                _accountingTypeDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private AccountingType _accountingTypeSelected;
        public AccountingType AccountingTypeSelected
        {
            get => _accountingTypeSelected;
            set
            {
                _accountingTypeSelected = value;
                AccountingTypeForEdit = (AccountingType)_accountingTypeSelected?.Clone() ?? new AccountingType();
                OnPropertyChanged();
            }
        }
        private AccountingType _accountingTypeForEdit;
        public AccountingType AccountingTypeForEdit
        {
            get => _accountingTypeForEdit;
            set
            {
                _accountingTypeForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => AccountingTypeSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => AccountingTypeSelected != null;
        public bool CanClear => AccountingTypesDelete.Count > 0;
        public bool CanRecover => AccountingTypeDeleteSelected != null;

        public AccountingTypeViewModel()
        {
            AccountingTypeForEdit = new AccountingType();
            AccountingTypes = new ObservableCollection<AccountingType>();
            AccountingTypesDelete = new ObservableCollection<AccountingType>();
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
            AccountingTypes.Add(AccountingTypeDeleteSelected);
            AccountingTypesDelete.Remove(AccountingTypeDeleteSelected);
        }
        public void Drop()
        {
            AccountingTypesDelete.Add(AccountingTypeSelected);
            AccountingTypes.Remove(AccountingTypeSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                AccountingTypeForEdit.Id = null;
                await DataSender.PostRequest(nameof(AccountingTypes), AccountingTypeForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in AccountingTypesDelete)
            {
                await DataSender.DeleteRequest(nameof(AccountingTypes), item.Id.Value);
            }
            AccountingTypesDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            AccountingTypes = await ApiConnector.GetAll<AccountingType>(nameof(AccountingTypes));
            foreach (var item in AccountingTypesDelete)
                AccountingTypes.Remove(AccountingTypes.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(AccountingTypes), AccountingTypeSelected.Id.Value, AccountingTypeForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(AccountingTypeForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(AccountingTypes), AccountingTypeSelected.Id.Value, AccountingTypeForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(AccountingTypes, nameof(AccountingTypes));
        }
    }
}
