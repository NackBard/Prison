﻿using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class ProsecutionViewModel : TableViewModel, ICRUD, ITableModel
    {
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

        private ObservableCollection<Prosecution> _prosecutionsDelete;
        public ObservableCollection<Prosecution> ProsecutionsDelete
        {
            get => _prosecutionsDelete;
            set
            {
                _prosecutionsDelete = value;
                OnPropertyChanged();
            }
        }

        private Prosecution _prosecutionDeleteSelected;
        public Prosecution ProsecutionDeleteSelected
        {
            get => _prosecutionDeleteSelected;
            set
            {
                _prosecutionDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Prosecution _prosecutionSelected;
        public Prosecution ProsecutionSelected
        {
            get => _prosecutionSelected;
            set
            {
                _prosecutionSelected = value;
                ProsecutionForEdit = (Prosecution)_prosecutionSelected?.Clone() ?? new Prosecution();
                OnPropertyChanged();
            }
        }
        private Prosecution _prosecutionForEdit;
        public Prosecution ProsecutionForEdit
        {
            get => _prosecutionForEdit;
            set
            {
                _prosecutionForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => ProsecutionSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => ProsecutionSelected != null;
        public bool CanClear => ProsecutionsDelete.Count > 0;
        public bool CanRecover => ProsecutionDeleteSelected != null;

        public ProsecutionViewModel()
        {
            ProsecutionForEdit = new Prosecution();
            Prosecutions = new ObservableCollection<Prosecution>();
            ProsecutionsDelete = new ObservableCollection<Prosecution>();
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
            Prosecutions.Add(ProsecutionDeleteSelected);
            ProsecutionsDelete.Remove(ProsecutionDeleteSelected);
        }
        public void Drop()
        {
            ProsecutionsDelete.Add(ProsecutionSelected);
            Prosecutions.Remove(ProsecutionSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                ProsecutionForEdit.Id = null;
                await DataSender.PostRequest(nameof(Prosecutions), ProsecutionForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in ProsecutionsDelete)
            {
                var s = await DataSender.DeleteRequest(nameof(Prosecutions), item.Id.Value);
            }
            ProsecutionsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Prosecutions = await ApiConnector.GetAll<Prosecution>(nameof(Prosecutions));
            foreach (var item in ProsecutionsDelete)
                Prosecutions.Remove(Prosecutions.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Prosecutions), ProsecutionSelected.Id.Value, ProsecutionForEdit);
            ReadAsync();
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ProsecutionForEdit.Name) && !string.IsNullOrEmpty(ProsecutionForEdit.Article);
        }
    }
}

