using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.MVVM.ViewModel
{
    class WarehouseViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Warehouse> _warehouses;
        public ObservableCollection<Warehouse> Warehouses
        {
            get => _warehouses;
            set
            {
                _warehouses = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Warehouse> _warehousesDelete;
        public ObservableCollection<Warehouse> WarehousesDelete
        {
            get => _warehousesDelete;
            set
            {
                _warehousesDelete = value;
                OnPropertyChanged();
            }
        }

        private Warehouse _warehouseDeleteSelected;
        public Warehouse WarehouseDeleteSelected
        {
            get => _warehouseDeleteSelected;
            set
            {
                _warehouseDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Warehouse _warehouseSelected;
        public Warehouse WarehouseSelected
        {
            get => _warehouseSelected;
            set
            {
                _warehouseSelected = value;
                WarehouseForEdit = (Warehouse)_warehouseSelected?.Clone() ?? new Warehouse();
                OnPropertyChanged();
            }
        }
        private Warehouse _warehouseForEdit;
        public Warehouse WarehouseForEdit
        {
            get => _warehouseForEdit;
            set
            {
                _warehouseForEdit = value;
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

        public bool CanDelete => WarehouseSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => WarehouseSelected != null;
        public bool CanClear => WarehousesDelete.Count > 0;
        public bool CanRecover => WarehouseDeleteSelected != null;

        public WarehouseViewModel()
        {
            WarehouseForEdit = new Warehouse();
            Warehouses = new ObservableCollection<Warehouse>();
            WarehousesDelete = new ObservableCollection<Warehouse>();
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
            WarehouseSelected = (Warehouse)WarehouseDeleteSelected.Clone();
            WarehouseForEdit.IsDeleted = false;
            UpdateAsync();
            Warehouses.Add(WarehouseDeleteSelected);
            WarehousesDelete.Remove(WarehouseDeleteSelected);
        }
        public void Drop()
        {
            WarehouseForEdit.IsDeleted = true;
            UpdateAsync();
            WarehousesDelete.Add(WarehouseSelected);
            Warehouses.Remove(WarehouseSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                WarehouseForEdit.Id = null;
                await DataSender.PostRequest(nameof(Warehouses), WarehouseForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in WarehousesDelete)
            {
                await DataSender.DeleteRequest(nameof(Warehouses), item.Id.Value);
            }
            WarehousesDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Products = await ApiConnector.GetAll<Product>(nameof(Products));
            WarehousesDelete = new ObservableCollection<Warehouse>();
            Warehouses = new ObservableCollection<Warehouse>();
            var all = await ApiConnector.GetAll<Warehouse>(nameof(Warehouses));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    WarehousesDelete.Add(item);
                else
                    Warehouses.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Warehouses), WarehouseSelected.Id.Value, WarehouseForEdit);
        }

        public bool Validate()
        {
            return WarehouseForEdit.Count > 0 && WarehouseForEdit.ProductId != null; 
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Warehouses), WarehouseSelected.Id.Value, WarehouseForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Warehouses, nameof(Warehouses));
        }
    }
}
