using Prison.Data;
using Prison.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AccountingDiningVisitView.xaml
    /// </summary>
    public partial class AccountingDiningVisitView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;

        public AccountingDiningVisitViewModel ViewModel => DataContext as AccountingDiningVisitViewModel;

        public AccountingDiningVisitView()
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
                ViewModel.AccountingDiningVisits = TableHelper.Sort(ViewModel.AccountingDiningVisits, sortBy, isAscending);
            }
            else
            {
                _sortedColumn = column;
                isAscending = false;
                ViewModel.AccountingDiningVisits = TableHelper.Sort(ViewModel.AccountingDiningVisits, sortBy, isAscending);
            }
        }
    }
}
