using Prison.Data;
using Prison.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AccessLevelView.xaml
    /// </summary>
    public partial class AccessLevelView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;

        public AccessLevelViewModel ViewModel => DataContext as AccessLevelViewModel;

        public AccessLevelView()
        {
            InitializeComponent();
        }

        private void ColumnSorting(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;

            string sortBy = column.Tag.ToString();
            if (_sortedColumn == column && !isAscending)
            {
                isAscending = true;
                ViewModel.AccessLevels = TableHelper.Sort(ViewModel.AccessLevels, sortBy, isAscending);
            }
            else
            {
                _sortedColumn = column;
                isAscending = false;
                ViewModel.AccessLevels = TableHelper.Sort(ViewModel.AccessLevels, sortBy, isAscending);
            }
        }
    }
}
