using System.Collections.ObjectModel;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class AccountingRehabilitationWorkViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<AccountingRehabilitationWork> _accountingRehabilitationWorks;
        public ObservableCollection<AccountingRehabilitationWork> AccountingRehabilitationWorks
        {
            get => _accountingRehabilitationWorks;
            set
            {
                _accountingRehabilitationWorks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AccountingRehabilitationWork> _accountingRehabilitationWorksDelete;
        public ObservableCollection<AccountingRehabilitationWork> AccountingRehabilitationWorksDelete
        {
            get => _accountingRehabilitationWorksDelete;
            set
            {
                _accountingRehabilitationWorksDelete = value;
                OnPropertyChanged();
            }
        }

        private AccountingRehabilitationWork _accountingRehabilitationWorkDeleteSelected;
        public AccountingRehabilitationWork AccountingRehabilitationWorkDeleteSelected
        {
            get => _accountingRehabilitationWorkDeleteSelected;
            set
            {
                _accountingRehabilitationWorkDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private AccountingRehabilitationWork _accountingRehabilitationWorkSelected;
        public AccountingRehabilitationWork AccountingRehabilitationWorkSelected
        {
            get => _accountingRehabilitationWorkSelected;
            set
            {
                _accountingRehabilitationWorkSelected = value;
                AccountingRehabilitationWorkForEdit = (AccountingRehabilitationWork)_accountingRehabilitationWorkSelected?.Clone() ?? new AccountingRehabilitationWork();
                OnPropertyChanged();
            }
        }
        private AccountingRehabilitationWork _accountingRehabilitationWorkForEdit;
        public AccountingRehabilitationWork AccountingRehabilitationWorkForEdit
        {
            get => _accountingRehabilitationWorkForEdit;
            set
            {
                _accountingRehabilitationWorkForEdit = value;
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

        public bool CanDelete => AccountingRehabilitationWorkSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => AccountingRehabilitationWorkSelected != null;
        public bool CanClear => AccountingRehabilitationWorksDelete.Count > 0;
        public bool CanRecover => AccountingRehabilitationWorkDeleteSelected != null;

        public AccountingRehabilitationWorkViewModel()
        {
            AccountingRehabilitationWorkForEdit = new AccountingRehabilitationWork();
            AccountingRehabilitationWorks = new ObservableCollection<AccountingRehabilitationWork>();
            AccountingRehabilitationWorksDelete = new ObservableCollection<AccountingRehabilitationWork>();
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
            AccountingRehabilitationWorkSelected = (AccountingRehabilitationWork)AccountingRehabilitationWorkDeleteSelected.Clone();
            AccountingRehabilitationWorkForEdit.IsDeleted = false;
            UpdateAsync();
            AccountingRehabilitationWorks.Add(AccountingRehabilitationWorkDeleteSelected);
            AccountingRehabilitationWorksDelete.Remove(AccountingRehabilitationWorkDeleteSelected);
        }
        public void Drop()
        {
            AccountingRehabilitationWorkForEdit.IsDeleted = true;
            UpdateAsync();
            AccountingRehabilitationWorksDelete.Add(AccountingRehabilitationWorkSelected);
            AccountingRehabilitationWorks.Remove(AccountingRehabilitationWorkSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                AccountingRehabilitationWorkForEdit.Id = null;
                await DataSender.PostRequest(nameof(AccountingRehabilitationWorks), AccountingRehabilitationWorkForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in AccountingRehabilitationWorksDelete)
                await DataSender.DeleteRequest(nameof(AccountingRehabilitationWorks), item.Id.Value);
            AccountingRehabilitationWorksDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Works = await ApiConnector.GetAll<Work>(nameof(Works));
            Prisoners = await ApiConnector.GetAll<Prisoner>(nameof(Prisoners));
            AccountingRehabilitationWorksDelete = new ObservableCollection<AccountingRehabilitationWork>();
            AccountingRehabilitationWorks = new ObservableCollection<AccountingRehabilitationWork>();
            var all = await ApiConnector.GetAll<AccountingRehabilitationWork>(nameof(AccountingRehabilitationWorks));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    AccountingRehabilitationWorksDelete.Add(item);
                else
                    AccountingRehabilitationWorks.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(AccountingRehabilitationWorks), AccountingRehabilitationWorkSelected.Id.Value, AccountingRehabilitationWorkForEdit);
        }

        public bool Validate()
        {
            return AccountingRehabilitationWorkForEdit.PrisonerId !=null && AccountingRehabilitationWorkForEdit.Salary >= 0 && AccountingRehabilitationWorkForEdit.WorkId !=null;
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(AccountingRehabilitationWorks), AccountingRehabilitationWorkSelected.Id.Value, AccountingRehabilitationWorkForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(AccountingRehabilitationWorks, nameof(AccountingRehabilitationWorks));
        }
    }
}


