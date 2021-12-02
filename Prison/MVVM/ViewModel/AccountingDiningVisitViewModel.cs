using System.Collections.ObjectModel;
using System.Linq;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class AccountingDiningVisitViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<AccountingDiningVisit> _accountingDiningVisits;
        public ObservableCollection<AccountingDiningVisit> AccountingDiningVisits
        {
            get => _accountingDiningVisits;
            set
            {
                _accountingDiningVisits = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AccountingDiningVisit> _accountingDiningVisitsDelete;
        public ObservableCollection<AccountingDiningVisit> AccountingDiningVisitsDelete
        {
            get => _accountingDiningVisitsDelete;
            set
            {
                _accountingDiningVisitsDelete = value;
                OnPropertyChanged();
            }
        }

        private AccountingDiningVisit _accountingDiningVisitDeleteSelected;
        public AccountingDiningVisit AccountingDiningVisitDeleteSelected
        {
            get => _accountingDiningVisitDeleteSelected;
            set
            {
                _accountingDiningVisitDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private AccountingDiningVisit _accountingDiningVisitSelected;
        public AccountingDiningVisit AccountingDiningVisitSelected
        {
            get => _accountingDiningVisitSelected;
            set
            {
                _accountingDiningVisitSelected = value;
                AccountingDiningVisitForEdit = (AccountingDiningVisit)_accountingDiningVisitSelected?.Clone() ?? new AccountingDiningVisit();
                OnPropertyChanged();
            }
        }
        private AccountingDiningVisit _accountingDiningVisitForEdit;
        public AccountingDiningVisit AccountingDiningVisitForEdit
        {
            get => _accountingDiningVisitForEdit;
            set
            {
                _accountingDiningVisitForEdit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Prisoner> _prisoners;
        public ObservableCollection<Prisoner> Prisoners
        {
            get => _prisoners;
            set
            {
                _prisoners = value;
                OnPropertyChanged();
            }
        }

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

        public bool CanDelete => AccountingDiningVisitSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => AccountingDiningVisitSelected != null;
        public bool CanClear => AccountingDiningVisitsDelete.Count > 0;
        public bool CanRecover => AccountingDiningVisitDeleteSelected != null;

        public AccountingDiningVisitViewModel()
        {
            AccountingDiningVisitForEdit = new AccountingDiningVisit();
            AccountingDiningVisits = new ObservableCollection<AccountingDiningVisit>();
            AccountingDiningVisitsDelete = new ObservableCollection<AccountingDiningVisit>();
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
        }
        public void Recover()
        {
            AccountingDiningVisits.Add(AccountingDiningVisitDeleteSelected);
            AccountingDiningVisitsDelete.Remove(AccountingDiningVisitDeleteSelected);
        }
        public void Drop()
        {
            AccountingDiningVisitsDelete.Add(AccountingDiningVisitSelected);
            AccountingDiningVisits.Remove(AccountingDiningVisitSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                AccountingDiningVisitForEdit.Id = null;
                await DataSender.PostRequest(nameof(AccountingDiningVisits), AccountingDiningVisitForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in AccountingDiningVisitsDelete)
                await DataSender.DeleteRequest(nameof(AccountingDiningVisits), item.Id.Value);
            AccountingDiningVisitsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Sets = await ApiConnector.GetAll<Set>(nameof(Sets));
            Prisoners = await ApiConnector.GetAll<Prisoner>(nameof(Prisoners));
            AccountingDiningVisits = await ApiConnector.GetAll<AccountingDiningVisit>(nameof(AccountingDiningVisits));
            foreach (var item in AccountingDiningVisitsDelete)
                AccountingDiningVisits.Remove(AccountingDiningVisits.First(level => level.Id == item.Id));
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(AccountingDiningVisits), AccountingDiningVisitSelected.Id.Value, AccountingDiningVisitForEdit);
            ReadAsync();
        }

        public bool Validate()
        {
            return false;
        }
    }
}

