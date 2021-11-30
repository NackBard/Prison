using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;
using System.Linq;
namespace Prison.MVVM.ViewModel
{
    class ProductViewModel : TableViewModel, ICRUD, ITableModel
    {
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

        private ObservableCollection<Product> _productsDelete;
        public ObservableCollection<Product> ProductsDelete
        {
            get => _productsDelete;
            set
            {
                _productsDelete = value;
                OnPropertyChanged();
            }
        }

        private Product _productDeleteSelected;
        public Product ProductDeleteSelected
        {
            get => _productDeleteSelected;
            set
            {
                _productDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Product _productSelected;
        public Product ProductSelected
        {
            get => _productSelected;
            set
            {
                _productSelected = value;
                ProductForEdit = (Product)_productSelected?.Clone() ?? new Product();
                OnPropertyChanged();
            }
        }
        private Product _productForEdit;
        public Product ProductForEdit
        {
            get => _productForEdit;
            set
            {
                _productForEdit = value;
                OnPropertyChanged();
            }
        }

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

        public bool CanDelete => ProductSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => ProductSelected != null;
        public bool CanClear => ProductsDelete.Count > 0;
        public bool CanRecover => ProductDeleteSelected != null;

        public ProductViewModel()
        {
            ProductForEdit = new Product();
            Products = new ObservableCollection<Product>();
            ProductsDelete = new ObservableCollection<Product>();
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
            Products.Add(ProductDeleteSelected);
            ProductsDelete.Remove(ProductDeleteSelected);
        }
        public void Drop()
        {
            ProductsDelete.Add(ProductSelected);
            Products.Remove(ProductSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                ProductForEdit.Id = null;
                await DataSender.PostRequest(nameof(Products), ProductForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in ProductsDelete)
            {
                var s = await DataSender.DeleteRequest(nameof(Products), item.Id.Value);
            }
            ProductsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            TypeProducts = await ApiConnector.GetAll<TypeProduct>(nameof(TypeProducts));
            Products = await ApiConnector.GetAll<Product>(nameof(Products));
            foreach (var item in ProductsDelete)
                Products.Remove(Products.Where(level => level.Id == item.Id).First());
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Products), ProductSelected.Id.Value, ProductForEdit);
            ReadAsync();
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ProductForEdit.Name) && ProductForEdit.ProductTypeId != null;
        }
    }
}
