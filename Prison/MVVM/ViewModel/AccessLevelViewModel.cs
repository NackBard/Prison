using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prison.Core;
using Prison.Data;
using Prison.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Prison.MVVM.ViewModel
{
    class AccessLevelViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<AccessLevel> _accessLevels;
        public ObservableCollection<AccessLevel> AccessLevels
        {
            get => _accessLevels;
            set
            {
                _accessLevels = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<AccessLevel> _accessLevelsDelete;
        public ObservableCollection<AccessLevel> AccessLevelsDelete
        {
            get => _accessLevelsDelete;
            set
            {
                _accessLevelsDelete = value;
                OnPropertyChanged();
            }
        }

        private AccessLevel _accessLevelDeleteSelected;
        public AccessLevel AccessLevelDeleteSelected
        {
            get => _accessLevelDeleteSelected;
            set
            {
                _accessLevelDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private AccessLevel _accessLevelSelected;
        public AccessLevel AccessLevelSelected
        {
            get => _accessLevelSelected;
            set
            {
                _accessLevelSelected = value;
                AccessLevelForEdit = (AccessLevel)_accessLevelSelected?.Clone() ?? new AccessLevel();
                OnPropertyChanged();
            }
        }
        private AccessLevel _accessLevelForEdit;
        public AccessLevel AccessLevelForEdit
        {
            get => _accessLevelForEdit;
            set
            {
                _accessLevelForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => AccessLevelSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => AccessLevelSelected != null;
        public bool CanClear => AccessLevelsDelete.Count > 0;
        public bool CanRecover => AccessLevelDeleteSelected != null;

        public AccessLevelViewModel()
        {
            AccessLevelForEdit = new AccessLevel();
            AccessLevels = new ObservableCollection<AccessLevel>();
            AccessLevelsDelete = new ObservableCollection<AccessLevel>();
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
            AccessLevels.Add(AccessLevelDeleteSelected);
            AccessLevelsDelete.Remove(AccessLevelDeleteSelected);
        }
        public void Drop()
        {
            AccessLevelsDelete.Add(AccessLevelSelected);
            AccessLevels.Remove(AccessLevelSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                AccessLevelForEdit.Id = null;
                await DataSender.PostRequest(nameof(AccessLevels), AccessLevelForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in AccessLevelsDelete)
            {
                await DataSender.DeleteRequest(nameof(AccessLevels), item.Id.Value);
            }
            AccessLevelsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            AccessLevels = await ApiConnector.GetAll<AccessLevel>(nameof(AccessLevels));
            AccessLevels = Sort(AccessLevels, nameof(AccessLevel.Name));
              foreach (var item in AccessLevelsDelete)
                AccessLevels.Remove(AccessLevels.Where(level => level.Id == item.Id).First());
        }

        public ObservableCollection<T> Sort<T>(ObservableCollection<T> sorter, string propertyName)
        {
            var s = sorter.OrderBy(a => a.GetType().GetProperty(propertyName).GetValue(a,null));
            sorter = new ObservableCollection<T>();
            foreach (var item in s)
                sorter.Add(item);
            return sorter;
        }

        //public ObservableCollection<T> Filter<T>(ObservableCollection<T> sorter, string propertyName, object value)
        //{
        //    var d = from sort in sorter where sort.GetType().GetProperty(propertyName).GetValue(sort) == value select sort;
        //    var s = sorter.Where(a =>
        //    a.GetType().GetProperty(propertyName).GetValue(a) == value &&
        //    value == value
        //    );
        //    sorter = new ObservableCollection<T>();
        //    foreach (var item in s)
        //        sorter.Add(item);
        //    return sorter;
        //}

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(AccessLevels), AccessLevelSelected.Id.Value, AccessLevelForEdit);
            ReadAsync();
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(AccessLevelForEdit.Name);
        }
    }
}
