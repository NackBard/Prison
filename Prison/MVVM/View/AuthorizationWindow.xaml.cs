using Prison.Data;
using Prison.Model;
using Prison.MVVM.Model;
using Prison.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private ObservableCollection<Post> Posts;

        public AuthorizationWindow()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            Posts = await ApiConnector.GetAll<Post>(nameof(Posts));
        }

        public async void Authorization(object sender, RoutedEventArgs e)
        {
            Auth.IsEnabled = false;
            if (Validate())
            {
                Auth.IsEnabled = false;
                return;
            }

            Worker worker = await ApiConnector.Authorization<Worker>(new Authorization { Login = LoginBox.Text, Password = PasswordBox.Password });

            Auth.IsEnabled = true;

            if (worker == null || worker?.Login != LoginBox.Text || worker?.Password != PasswordBox.Password)
                return;

            var post = Posts.FirstOrDefault(p => p.Id == worker.PostId);

            MainWindow window = new MainWindow();

            switch (post.Name.Trim())
            {
                case "Работник кухни":
                    window.ViewModel.views = new object[] { new DishViewModel(), new AccountingDiningVisitViewModel(), new SetViewModel() };
                    window.Show();
                    Close();
                    break;
                case "Бухгалтер":
                    window.ViewModel.views = new object[] {
                        new SalesAccountingViewModel(),
                        new WorkerViewModel(),
                        new PrisonerViewModel(),
                        new AccountingDiningVisitViewModel(),
                        new AccountingPrisonerViewModel(),
                        new AccountingRehabilitationWorkViewModel(),
                        new JournalArrivalAndDepartureViewModel(),
                    };
                    window.Show();
                    Close();
                    break;
                case "Продавец":
                    window.ViewModel.views = new object[] { new SalesAccountingViewModel(), new ProductViewModel() };
                    window.Show();
                    Close();
                    break;
                case "Член комиссии УДО":
                    window.ViewModel.views = new object[] {
                        new AccountingDiningVisitViewModel(),
                        new AccountingPrisonerViewModel(),
                        new AccountingRehabilitationWorkViewModel(),
                        new SalesAccountingViewModel()
                    };
                    window.Show();
                    Close();
                    break;
                case "Сотрудник склада":
                    window.ViewModel.views = new object[] {
                        new TypeProductViewModel(),
                        new WarehouseViewModel(),
                        new ProductViewModel()
                    };
                    window.Show();
                    Close();
                    break;
                case "Администратор":
                    window.Show();
                    Close();
                    break;
                default:
                    MessageBox.Show("Нету доступа");
                    window.Close();
                    break;
            }
        }

        private bool Validate()
        {
            return string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password);
        }
    }
}
