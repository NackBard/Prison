using Prison.Core;
using Prison.Data;
using Prison.Model;
using System;
using System.Collections.ObjectModel;

namespace Prison.MVVM.ViewModel
{
    class PrisonerViewModel : TableViewModel, ICRUD, ITableModel
    {
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

        private ObservableCollection<Prisoner> _prisonersDelete;
        public ObservableCollection<Prisoner> PrisonersDelete
        {
            get => _prisonersDelete;
            set
            {
                _prisonersDelete = value;
                OnPropertyChanged();
            }
        }

        private Prisoner _prisonerDeleteSelected;
        public Prisoner PrisonerDeleteSelected
        {
            get => _prisonerDeleteSelected;
            set
            {
                _prisonerDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Prisoner _prisonerSelected;
        public Prisoner PrisonerSelected
        {
            get => _prisonerSelected;
            set
            {
                _prisonerSelected = value;
                PrisonerForEdit = (Prisoner)_prisonerSelected?.Clone() ?? new Prisoner();
                OnPropertyChanged();
            }
        }
        private Prisoner _prisonerForEdit;
        public Prisoner PrisonerForEdit
        {
            get => _prisonerForEdit;
            set
            {
                _prisonerForEdit = value;
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

        private ObservableCollection<Prosecution> _prosecutions;
        public ObservableCollection<Prosecution> Prosecutions
        {
            get => _prosecutions;
            set
            {
                _prosecutions = value;
                OnPropertyChanged();
            }
        }

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

        public bool CanDelete => PrisonerSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => PrisonerSelected != null;
        public bool CanClear => PrisonersDelete.Count > 0;
        public bool CanRecover => PrisonerDeleteSelected != null;

        public PrisonerViewModel()
        {
            PrisonerForEdit = new Prisoner();
            Prisoners = new ObservableCollection<Prisoner>();
            PrisonersDelete = new ObservableCollection<Prisoner>();
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
            PrisonerSelected = (Prisoner)PrisonerDeleteSelected.Clone();
            PrisonerForEdit.IsDeleted = false;
            UpdateAsync();
            Prisoners.Add(PrisonerDeleteSelected);
            PrisonersDelete.Remove(PrisonerDeleteSelected);
        }
        public void Drop()
        {
            PrisonerForEdit.IsDeleted = true;
            UpdateAsync();
            PrisonersDelete.Add(PrisonerSelected);
            Prisoners.Remove(PrisonerSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                PrisonerForEdit.Id = null;
                await DataSender.PostRequest(nameof(Prisoners), PrisonerForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in PrisonersDelete)
                await DataSender.DeleteRequest(nameof(Prisoners), item.Id.Value);
            PrisonersDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Statuses = await ApiConnector.GetAll<Status>(nameof(Status));
            Prosecutions = await ApiConnector.GetAll<Prosecution>(nameof(Prosecutions));
            Genders = await ApiConnector.GetAll<Gender>(nameof(Genders));
            PrisonersDelete = new ObservableCollection<Prisoner>();
            Prisoners = new ObservableCollection<Prisoner>();
            var all = await ApiConnector.GetAll<Prisoner>(nameof(Prisoners));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    PrisonersDelete.Add(item);
                else
                    Prisoners.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Prisoners), PrisonerSelected.Id.Value, PrisonerForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(PrisonerForEdit.Verdict) && !string.IsNullOrWhiteSpace(PrisonerForEdit.Surname) && !string.IsNullOrWhiteSpace(PrisonerForEdit.Name) &&
                PrisonerForEdit.StatusId != null && PrisonerForEdit.ProsecutionId != null && PrisonerForEdit.GenderId != null &&
                PrisonerForEdit.DateOfBirthday.Year <= DateTime.Now.Year - 18 && PrisonerForEdit.DateOfBirthday.Year >= DateTime.Now.Year - 100 &&
                PrisonerForEdit.DateOfConclusion.Year >= DateTime.Now.Year - 100;
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Prisoners), PrisonerSelected.Id.Value, PrisonerForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Prisoners, nameof(Prisoners));
        }
    }
}
