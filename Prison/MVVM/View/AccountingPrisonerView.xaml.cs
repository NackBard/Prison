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
using Prison.Data;
using Prison.MVVM.ViewModel;

namespace Prison.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AccountingPrisonerView.xaml
    /// </summary>
    public partial class AccountingPrisonerView : UserControl
    {
        private GridViewColumnHeader _sortedColumn;
        private bool isAscending;

        public AccountingPrisonerViewModel ViewModel => DataContext as AccountingPrisonerViewModel;

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
                ViewModel.AccountingPrisoners = TableHelper.Sort(ViewModel.AccountingPrisoners, sortBy, isAscending);
            }
            else
            {
                _sortedColumn = column;
                isAscending = false;
                ViewModel.AccountingPrisoners = TableHelper.Sort(ViewModel.AccountingPrisoners, sortBy, isAscending);
            }
        }
    }
}
