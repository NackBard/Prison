using System.Collections.ObjectModel;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class DishViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Dish> _dishes;
        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set
            {
                _dishes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Dish> _dishesDelete;
        public ObservableCollection<Dish> DishesDelete
        {
            get => _dishesDelete;
            set
            {
                _dishesDelete = value;
                OnPropertyChanged();
            }
        }

        private Dish _dishDeleteSelected;
        public Dish DishDeleteSelected
        {
            get => _dishDeleteSelected;
            set
            {
                _dishDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Dish _dishSelected;
        public Dish DishSelected
        {
            get => _dishSelected;
            set
            {
                _dishSelected = value;
                DishForEdit = (Dish)_dishSelected?.Clone() ?? new Dish();
                OnPropertyChanged();
            }
        }
        private Dish _dishForEdit;
        public Dish DishForEdit
        {
            get => _dishForEdit;
            set
            {
                _dishForEdit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Set> _sets;
        public ObservableCollection<Set> Sets
        {
            get => _sets;
            set
            {
                _sets = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => DishSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => DishSelected != null;
        public bool CanClear => DishesDelete.Count > 0;
        public bool CanRecover => DishDeleteSelected != null;

        public DishViewModel()
        {
            DishForEdit = new Dish();
            Dishes = new ObservableCollection<Dish>();
            DishesDelete = new ObservableCollection<Dish>();
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
            ExportCommand = new RelayCommand(o => Export());
        }
        public void Recover()
        {
            DishSelected = (Dish)DishDeleteSelected.Clone();
            DishForEdit.IsDeleted = false;
            UpdateAsync();
            Dishes.Add(DishDeleteSelected);
            DishesDelete.Remove(DishDeleteSelected);
        }
        public void Drop()
        {
            DishForEdit.IsDeleted = true;
            UpdateAsync();
            DishesDelete.Add(DishSelected);
            Dishes.Remove(DishSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                DishForEdit.Id = null;
                await DataSender.PostRequest(nameof(Dishes), DishForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in DishesDelete)
                await DataSender.DeleteRequest(nameof(Dishes), item.Id.Value);
            DishesDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            Sets = await ApiConnector.GetAll<Set>(nameof(Sets));
            DishesDelete = new ObservableCollection<Dish>();
            Dishes = new ObservableCollection<Dish>();
            var all = await ApiConnector.GetAll<Dish>(nameof(Dishes));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    DishesDelete.Add(item);
                else
                    Dishes.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Dishes), DishSelected.Id.Value, DishForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(DishForEdit.Name) && DishForEdit.SetId != null;
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Dishes), DishSelected.Id.Value, DishForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(Dishes, nameof(Dishes));
        }
    }
}


