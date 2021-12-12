using Prison.Data;
using Prison.Model;
using Prison.MVVM.Model;
using System.Windows;
using System.Windows.Controls;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        public async void Authorization(object sender, RoutedEventArgs e)
        {
            if (Validate())
                return;

            Worker worker = await ApiConnector.Authorization<Worker>(new Authorization { Login = LoginBox.Text, Password = PasswordBox.Password });
            if (worker == null || worker?.Login != LoginBox.Text || worker?.Password != PasswordBox.Password)
                return;

            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private bool Validate()
        {
            return string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password);
        }
    }
}
