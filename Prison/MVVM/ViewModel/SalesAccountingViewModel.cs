﻿using Prison.Core;
using Prison.Data;
using Prison.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prison.MVVM.ViewModel
{
    class SalesAccountingViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<SalesAccounting> _salesAccountings;
        public ObservableCollection<SalesAccounting> SalesAccountings
        {
            get => _salesAccountings;
            set
            {
                _salesAccountings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SalesAccounting> _salesAccountingsDelete;
        public ObservableCollection<SalesAccounting> SalesAccountingsDelete
        {
            get => _salesAccountingsDelete;
            set
            {
                _salesAccountingsDelete = value;
                OnPropertyChanged();
            }
        }

        private SalesAccounting _salesAccountingDeleteSelected;
        public SalesAccounting SalesAccountingDeleteSelected
        {
            get => _salesAccountingDeleteSelected;
            set
            {
                _salesAccountingDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private SalesAccounting _salesAccountingSelected;
        public SalesAccounting SalesAccountingSelected
        {
            get => _salesAccountingSelected;
            set
            {
                _salesAccountingSelected = value;
                SalesAccountingForEdit = (SalesAccounting)_salesAccountingSelected?.Clone() ?? new SalesAccounting();
                OnPropertyChanged();
            }
        }
        private SalesAccounting _salesAccountingForEdit;
        public SalesAccounting SalesAccountingForEdit
        {
            get => _salesAccountingForEdit;
            set
            {
                _salesAccountingForEdit = value;
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

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }


        public bool CanDelete => SalesAccountingSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => SalesAccountingSelected != null;
        public bool CanClear => SalesAccountingsDelete.Count > 0;
        public bool CanRecover => SalesAccountingDeleteSelected != null;

        public SalesAccountingViewModel()
        {
            SalesAccountingForEdit = new SalesAccounting();
            SalesAccountings = new ObservableCollection<SalesAccounting>();
            SalesAccountingsDelete = new ObservableCollection<SalesAccounting>();
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
            SalesAccountings.Add(SalesAccountingDeleteSelected);
            SalesAccountingsDelete.Remove(SalesAccountingDeleteSelected);
        }
        public void Drop()
        {
            SalesAccountingsDelete.Add(SalesAccountingSelected);
            SalesAccountings.Remove(SalesAccountingSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                SalesAccountingForEdit.Id = null;
                SalesAccountingForEdit.Total = ProductCalculate();
                SalesAccountingForEdit.Date = DateTime.Now;
                await DataSender.PostRequest(nameof(SalesAccountings), SalesAccountingForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in SalesAccountingsDelete)
                await DataSender.DeleteRequest(nameof(SalesAccountings), item.Id.Value);
            SalesAccountingsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Products = await ApiConnector.GetAll<Product>(nameof(Products));
            Prisoners = await ApiConnector.GetAll<Prisoner>(nameof(Prisoners));
            SalesAccountings = await ApiConnector.GetAll<SalesAccounting>(nameof(SalesAccountings));
            foreach (var item in SalesAccountingsDelete)
                SalesAccountings.Remove(SalesAccountings.First(level => level.Id == item.Id));
        }

        public async void UpdateAsync()
        {
            SalesAccountingForEdit.Total = ProductCalculate();
            SalesAccountingForEdit.Date = DateTime.Now;
            await DataSender.PutRequest(nameof(SalesAccountings), SalesAccountingSelected.Id.Value, SalesAccountingForEdit);
            ReadAsync();
        }

        public bool Validate()
        {
            return false;
        }

        public double ProductCalculate() => SalesAccountingForEdit.Count * 10;
    }
}
