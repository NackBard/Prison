using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AccountingPrisonerView.xaml
    /// </summary>
    public partial class AccountingPrisonerView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;

        public AccountingDiningVisitViewModel ViewModel => DataContext as AccountingDiningVisitViewModel;

        public AccountingPrisonerView()
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
