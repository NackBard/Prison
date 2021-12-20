using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;

namespace Prison.MVVM.ViewModel
{
    class TypeProductViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<TypeProduct> _typeProducts;
        public ObservableCollection<TypeProduct> TypeProducts
        {
            get => _typeProducts;
            set
            {
                _typeProducts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TypeProduct> _typeProductsDelete;
        public ObservableCollection<TypeProduct> TypeProductsDelete
        {
            get => _typeProductsDelete;
            set
            {
                _typeProductsDelete = value;
                OnPropertyChanged();
            }
        }

        private TypeProduct _typeProductDeleteSelected;
        public TypeProduct TypeProductDeleteSelected
        {
            get => _typeProductDeleteSelected;
            set
            {
                _typeProductDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private TypeProduct _typeProductSelected;
        public TypeProduct TypeProductSelected
        {
            get => _typeProductSelected;
            set
            {
                _typeProductSelected = value;
                TypeProductForEdit = (TypeProduct)_typeProductSelected?.Clone() ?? new TypeProduct();
                OnPropertyChanged();
            }
        }
        private TypeProduct _typeProductForEdit;
        public TypeProduct TypeProductForEdit
        {
            get => _typeProductForEdit;
            set
            {
                _typeProductForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => TypeProductSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => TypeProductSelected != null;
        public bool CanClear => TypeProductsDelete.Count > 0;
        public bool CanRecover => TypeProductDeleteSelected != null;

        public TypeProductViewModel()
        {
            TypeProductForEdit = new TypeProduct();
            TypeProducts = new ObservableCollection<TypeProduct>();
            TypeProductsDelete = new ObservableCollection<TypeProduct>();
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
            TypeProductSelected = (TypeProduct)TypeProductDeleteSelected.Clone();
            TypeProductForEdit.IsDeleted = false;
            UpdateAsync();
            TypeProducts.Add(TypeProductDeleteSelected);
            TypeProductsDelete.Remove(TypeProductDeleteSelected);
        }
        public void Drop()
        {
            TypeProductForEdit.IsDeleted = true;
            UpdateAsync();
            TypeProductsDelete.Add(TypeProductSelected);
            TypeProducts.Remove(TypeProductSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                TypeProductForEdit.Id = null;
                await DataSender.PostRequest(nameof(TypeProducts), TypeProductForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in TypeProductsDelete)
            {
                await DataSender.DeleteRequest(nameof(TypeProducts), item.Id.Value);
            }
            TypeProductsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            TypeProductsDelete = new ObservableCollection<TypeProduct>();
            TypeProducts = new ObservableCollection<TypeProduct>();
            var all = await ApiConnector.GetAll<TypeProduct>(nameof(TypeProducts));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    TypeProductsDelete.Add(item);
                else
                    TypeProducts.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(TypeProducts), TypeProductSelected.Id.Value, TypeProductForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(TypeProductForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(TypeProducts), TypeProductSelected.Id.Value, TypeProductForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(TypeProducts, nameof(TypeProducts));
        }
    }
}