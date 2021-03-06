using System;
using System.Collections.ObjectModel;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class JournalArrivalAndDepartureViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<JournalArrivalAndDeparture> _journalArrivalAndDepartures;
        public ObservableCollection<JournalArrivalAndDeparture> JournalArrivalAndDepartures
        {
            get => _journalArrivalAndDepartures;
            set
            {
                _journalArrivalAndDepartures = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<JournalArrivalAndDeparture> _journalArrivalAndDeparturesDelete;
        public ObservableCollection<JournalArrivalAndDeparture> JournalArrivalAndDeparturesDelete
        {
            get => _journalArrivalAndDeparturesDelete;
            set
            {
                _journalArrivalAndDeparturesDelete = value;
                OnPropertyChanged();
            }
        }

        private JournalArrivalAndDeparture _journalArrivalAndDepartureDeleteSelected;
        public JournalArrivalAndDeparture JournalArrivalAndDepartureDeleteSelected
        {
            get => _journalArrivalAndDepartureDeleteSelected;
            set
            {
                _journalArrivalAndDepartureDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private JournalArrivalAndDeparture _journalArrivalAndDepartureSelected;
        public JournalArrivalAndDeparture JournalArrivalAndDepartureSelected
        {
            get => _journalArrivalAndDepartureSelected;
            set
            {
                _journalArrivalAndDepartureSelected = value;
                JournalArrivalAndDepartureForEdit = (JournalArrivalAndDeparture)_journalArrivalAndDepartureSelected?.Clone() ?? new JournalArrivalAndDeparture();
                OnPropertyChanged();
            }
        }
        private JournalArrivalAndDeparture _journalArrivalAndDepartureForEdit;
        public JournalArrivalAndDeparture JournalArrivalAndDepartureForEdit
        {
            get => _journalArrivalAndDepartureForEdit;
            set
            {
                _journalArrivalAndDepartureForEdit = value;
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

        public bool CanDelete => JournalArrivalAndDepartureSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => JournalArrivalAndDepartureSelected != null;
        public bool CanClear => JournalArrivalAndDeparturesDelete.Count > 0;
        public bool CanRecover => JournalArrivalAndDepartureDeleteSelected != null;

        public JournalArrivalAndDepartureViewModel()
        {
            JournalArrivalAndDepartureForEdit = new JournalArrivalAndDeparture();
            JournalArrivalAndDepartures = new ObservableCollection<JournalArrivalAndDeparture>();
            JournalArrivalAndDeparturesDelete = new ObservableCollection<JournalArrivalAndDeparture>();
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
            JournalArrivalAndDepartureSelected = (JournalArrivalAndDeparture)JournalArrivalAndDepartureDeleteSelected.Clone();
            JournalArrivalAndDepartureForEdit.IsDeleted = false;
            UpdateAsync();
            JournalArrivalAndDepartures.Add(JournalArrivalAndDepartureDeleteSelected);
            JournalArrivalAndDeparturesDelete.Remove(JournalArrivalAndDepartureDeleteSelected);
        }
        public void Drop()
        {
            JournalArrivalAndDepartureForEdit.IsDeleted = true;
            UpdateAsync();
            JournalArrivalAndDeparturesDelete.Add(JournalArrivalAndDepartureSelected);
            JournalArrivalAndDepartures.Remove(JournalArrivalAndDepartureSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                JournalArrivalAndDepartureForEdit.Date = DateTime.Now;
                JournalArrivalAndDepartureForEdit.Id = null;
                await DataSender.PostRequest(nameof(JournalArrivalAndDepartures), JournalArrivalAndDepartureForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in JournalArrivalAndDeparturesDelete)
                await DataSender.DeleteRequest(nameof(JournalArrivalAndDepartures), item.Id.Value);
            JournalArrivalAndDeparturesDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            AccountingTypes = await ApiConnector.GetAll<AccountingType>(nameof(AccountingTypes));
            Workers = await ApiConnector.GetAll<Worker>(nameof(Workers));
            JournalArrivalAndDeparturesDelete = new ObservableCollection<JournalArrivalAndDeparture>();
            JournalArrivalAndDepartures = new ObservableCollection<JournalArrivalAndDeparture>();
            var all = await ApiConnector.GetAll<JournalArrivalAndDeparture>(nameof(JournalArrivalAndDepartures));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    JournalArrivalAndDeparturesDelete.Add(item);
                else
                    JournalArrivalAndDepartures.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            JournalArrivalAndDepartureForEdit.Date = DateTime.Now;
            await DataSender.PutRequest(nameof(JournalArrivalAndDepartures), JournalArrivalAndDepartureSelected.Id.Value, JournalArrivalAndDepartureForEdit);
        }

        public bool Validate()
        {
            return JournalArrivalAndDepartureForEdit.WorkerId != null && JournalArrivalAndDepartureForEdit.AccountingTypeId != null;
        }

        public async void UpdateWithReadAsync()
        {
            JournalArrivalAndDepartureForEdit.Date = DateTime.Now;
            await DataSender.PutRequest(nameof(JournalArrivalAndDepartures), JournalArrivalAndDepartureSelected.Id.Value, JournalArrivalAndDepartureForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(JournalArrivalAndDepartures, nameof(JournalArrivalAndDepartures));
        }
    }
}


