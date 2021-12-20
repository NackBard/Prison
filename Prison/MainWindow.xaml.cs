using Prison.MVVM.ViewModel;
using System.Windows;

namespace Prison
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;
    }
}
