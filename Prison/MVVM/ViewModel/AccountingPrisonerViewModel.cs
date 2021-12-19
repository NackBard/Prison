using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class AccountingPrisonerViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<AccountingPrisoner> _accountingPrisoners;
        public ObservableCollection<AccountingPrisoner> AccountingPrisoners
        {
            get => _accountingPrisoners;
            set
            {
                _accountingPrisoners = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AccountingPrisoner> _accountingPrisonersDelete;
        public ObservableCollection<AccountingPrisoner> AccountingPrisonersDelete
        {
            get => _accountingPrisonersDelete;
            set
            {
                _accountingPrisonersDelete = value;
                OnPropertyChanged();
            }
        }

        private AccountingPrisoner _accountingPrisonerDeleteSelected;
        public AccountingPrisoner AccountingPrisonerDeleteSelected
        {
            get => _accountingPrisonerDeleteSelected;
            set
            {
                _accountingPrisonerDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private AccountingPrisoner _accountingPrisonerSelected;
        public AccountingPrisoner AccountingPrisonerSelected
        {
            get => _accountingPrisonerSelected;
            set
            {
                _accountingPrisonerSelected = value;
                AccountingPrisonerForEdit = (AccountingPrisoner)_accountingPrisonerSelected?.Clone() ?? new AccountingPrisoner();
                OnPropertyChanged();
            }
        }
        private AccountingPrisoner _accountingPrisonerForEdit;
        public AccountingPrisoner AccountingPrisonerForEdit
        {
            get => _accountingPrisonerForEdit;
            set
            {
                _accountingPrisonerForEdit = value;
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

        private ObservableCollection<BehaviorAssessment> _behaviorAssessments;
        public ObservableCollection<BehaviorAssessment> BehaviorAssessments
        {
            get => _behaviorAssessments;
            set
            {
                _behaviorAssessments = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => AccountingPrisonerSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => AccountingPrisonerSelected != null;
        public bool CanClear => AccountingPrisonersDelete.Count > 0;
        public bool CanRecover => AccountingPrisonerDeleteSelected != null;

        public AccountingPrisonerViewModel()
        {
            AccountingPrisonerForEdit = new AccountingPrisoner();
            AccountingPrisoners = new ObservableCollection<AccountingPrisoner>();
            AccountingPrisonersDelete = new ObservableCollection<AccountingPrisoner>();
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
            AccountingPrisonerSelected = (AccountingPrisoner)AccountingPrisonerDeleteSelected.Clone();
            AccountingPrisonerForEdit.IsDeleted = false;
            UpdateAsync();
            AccountingPrisoners.Add(AccountingPrisonerDeleteSelected);
            AccountingPrisonersDelete.Remove(AccountingPrisonerDeleteSelected);
        }
        public void Drop()
        {
            AccountingPrisonerForEdit.IsDeleted = true;
            UpdateAsync();
            AccountingPrisonersDelete.Add(AccountingPrisonerSelected);
            AccountingPrisoners.Remove(AccountingPrisonerSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                AccountingPrisonerForEdit.Id = null;
                AccountingPrisonerForEdit.DateOfEntry = DateTime.Now;
                await DataSender.PostRequest(nameof(AccountingPrisoners), AccountingPrisonerForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in AccountingPrisonersDelete)
                await DataSender.DeleteRequest(nameof(AccountingPrisoners), item.Id.Value);
            AccountingPrisonersDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            BehaviorAssessments = await ApiConnector.GetAll<BehaviorAssessment>(nameof(BehaviorAssessments));
            Workers = await ApiConnector.GetAll<Worker>(nameof(Workers));
            Prisoners = await ApiConnector.GetAll<Prisoner>(nameof(Prisoners));
            AccountingPrisonersDelete = new ObservableCollection<AccountingPrisoner>();
            AccountingPrisoners = new ObservableCollection<AccountingPrisoner>();
            var all = await ApiConnector.GetAll<AccountingPrisoner>(nameof(AccountingPrisoners));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    AccountingPrisonersDelete.Add(item);
                else
                    AccountingPrisoners.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            AccountingPrisonerForEdit.DateOfEntry = DateTime.Now;
            await DataSender.PutRequest(nameof(AccountingPrisoners), AccountingPrisonerSelected.Id.Value, AccountingPrisonerForEdit);
        }

        public bool Validate()
        {
            return AccountingPrisonerForEdit.PrisonerId !=null && AccountingPrisonerForEdit.WorkerId !=null &&
                AccountingPrisonerForEdit.AssessmentId !=null && !string.IsNullOrWhiteSpace(AccountingPrisonerForEdit.Content);
        }

        public async void UpdateWithReadAsync()
        {
            AccountingPrisonerForEdit.DateOfEntry = DateTime.Now;
            await DataSender.PutRequest(nameof(AccountingPrisoners), AccountingPrisonerSelected.Id.Value, AccountingPrisonerForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(AccountingPrisoners, nameof(AccountingPrisoners));
        }
    }
}


