using System.Collections.ObjectModel;
using System.Linq;
using Prison.Core;
using Prison.Data;
using Prison.Model;

namespace Prison.MVVM.ViewModel
{
    class PostViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> _postsDelete;
        public ObservableCollection<Post> PostsDelete
        {
            get => _postsDelete;
            set
            {
                _postsDelete = value;
                OnPropertyChanged();
            }
        }

        private Post _dishDeleteSelected;
        public Post PostDeleteSelected
        {
            get => _dishDeleteSelected;
            set
            {
                _dishDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private Post _dishSelected;
        public Post PostSelected
        {
            get => _dishSelected;
            set
            {
                _dishSelected = value;
                PostForEdit = (Post)_dishSelected?.Clone() ?? new Post();
                OnPropertyChanged();
            }
        }
        private Post _dishForEdit;
        public Post PostForEdit
        {
            get => _dishForEdit;
            set
            {
                _dishForEdit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Set> _accessLevels;
        public ObservableCollection<Set> AccessLevels
        {
            get => _accessLevels;
            set
            {
                _accessLevels = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => PostSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => PostSelected != null;
        public bool CanClear => PostsDelete.Count > 0;
        public bool CanRecover => PostDeleteSelected != null;

        public PostViewModel()
        {
            PostForEdit = new Post();
            Posts = new ObservableCollection<Post>();
            PostsDelete = new ObservableCollection<Post>();
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
        }
        public void Recover()
        {
            Posts.Add(PostDeleteSelected);
            PostsDelete.Remove(PostDeleteSelected);
        }
        public void Drop()
        {
            PostsDelete.Add(PostSelected);
            Posts.Remove(PostSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                PostForEdit.Id = null;
                await DataSender.PostRequest(nameof(Posts), PostForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in PostsDelete)
                await DataSender.DeleteRequest(nameof(Posts), item.Id.Value);
            PostsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            AccessLevels = await ApiConnector.GetAll<Set>(nameof(AccessLevels));
            Posts = await ApiConnector.GetAll<Post>(nameof(Posts));
            foreach (var item in PostsDelete)
                Posts.Remove(Posts.First(level => level.Id == item.Id));
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(Posts), PostSelected.Id.Value, PostForEdit);
        }

        public bool Validate()
        {
            return false;
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(Posts), PostSelected.Id.Value, PostForEdit);
            ReadAsync();
        }
    }
}



