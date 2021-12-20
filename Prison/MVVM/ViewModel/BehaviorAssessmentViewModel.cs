using Prison.Core;
using Prison.Data;
using Prison.Model;
using System.Collections.ObjectModel;

namespace Prison.MVVM.ViewModel
{
    class BehaviorAssessmentViewModel : TableViewModel, ICRUD, ITableModel
    {
        private ObservableCollection<BehaviorAssessment> _behaviorAssessments;
        public ObservableCollection<BehaviorAssessment> BehaviorAssessments
        {
            get => _behaviorAssessments;
            set
            {
                _behaviorAssessments = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BehaviorAssessment> _behaviorAssessmentsDelete;
        public ObservableCollection<BehaviorAssessment> BehaviorAssessmentsDelete
        {
            get => _behaviorAssessmentsDelete;
            set
            {
                _behaviorAssessmentsDelete = value;
                OnPropertyChanged();
            }
        }

        private BehaviorAssessment _behaviorAssessmentDeleteSelected;
        public BehaviorAssessment BehaviorAssessmentDeleteSelected
        {
            get => _behaviorAssessmentDeleteSelected;
            set
            {
                _behaviorAssessmentDeleteSelected = value;
                OnPropertyChanged();
            }
        }

        private BehaviorAssessment _behaviorAssessmentSelected;
        public BehaviorAssessment BehaviorAssessmentSelected
        {
            get => _behaviorAssessmentSelected;
            set
            {
                _behaviorAssessmentSelected = value;
                BehaviorAssessmentForEdit = (BehaviorAssessment)_behaviorAssessmentSelected?.Clone() ?? new BehaviorAssessment();
                OnPropertyChanged();
            }
        }
        private BehaviorAssessment _behaviorAssessmentForEdit;
        public BehaviorAssessment BehaviorAssessmentForEdit
        {
            get => _behaviorAssessmentForEdit;
            set
            {
                _behaviorAssessmentForEdit = value;
                OnPropertyChanged();
            }
        }

        public bool CanDelete => BehaviorAssessmentSelected != null;
        public bool CanCreate => Validate();
        public bool CanUpdate => BehaviorAssessmentSelected != null;
        public bool CanClear => BehaviorAssessmentsDelete.Count > 0;
        public bool CanRecover => BehaviorAssessmentDeleteSelected != null;

        public BehaviorAssessmentViewModel()
        {
            BehaviorAssessmentForEdit = new BehaviorAssessment();
            BehaviorAssessments = new ObservableCollection<BehaviorAssessment>();
            BehaviorAssessmentsDelete = new ObservableCollection<BehaviorAssessment>();
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
            BehaviorAssessmentSelected = (BehaviorAssessment)BehaviorAssessmentDeleteSelected.Clone();
            BehaviorAssessmentForEdit.IsDeleted = false;
            UpdateAsync();
            BehaviorAssessments.Add(BehaviorAssessmentDeleteSelected);
            BehaviorAssessmentsDelete.Remove(BehaviorAssessmentDeleteSelected);
        }
        public void Drop()
        {
            BehaviorAssessmentForEdit.IsDeleted = true;
            UpdateAsync();
            BehaviorAssessmentsDelete.Add(BehaviorAssessmentSelected);
            BehaviorAssessments.Remove(BehaviorAssessmentSelected);
        }

        public async void CreateAsync()
        {
            if (CanCreate)
            {
                BehaviorAssessmentForEdit.Id = null;
                await DataSender.PostRequest(nameof(BehaviorAssessments), BehaviorAssessmentForEdit);
                ReadAsync();
            }
        }

        public async void DeleteAsync()
        {
            foreach (var item in BehaviorAssessmentsDelete)
            {
                await DataSender.DeleteRequest(nameof(BehaviorAssessments), item.Id.Value);
            }
            BehaviorAssessmentsDelete.Clear();
            ReadAsync();
        }

        public async void ReadAsync()
        {
            BehaviorAssessmentsDelete = new ObservableCollection<BehaviorAssessment>();
            BehaviorAssessments = new ObservableCollection<BehaviorAssessment>();
            var all = await ApiConnector.GetAll<BehaviorAssessment>(nameof(BehaviorAssessments));
            foreach (var item in all)
            {
                if (item.IsDeleted)
                    BehaviorAssessmentsDelete.Add(item);
                else
                    BehaviorAssessments.Add(item);
            }
        }

        public async void UpdateAsync()
        {
            await DataSender.PutRequest(nameof(BehaviorAssessments), BehaviorAssessmentSelected.Id.Value, BehaviorAssessmentForEdit);
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(BehaviorAssessmentForEdit.Name);
        }

        public async void UpdateWithReadAsync()
        {
            await DataSender.PutRequest(nameof(BehaviorAssessments), BehaviorAssessmentSelected.Id.Value, BehaviorAssessmentForEdit);
            ReadAsync();
        }

        public void Export()
        {
            TableHelper.Export(BehaviorAssessments, nameof(BehaviorAssessments));
        }
    }
}

