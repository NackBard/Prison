using System.Collections.ObjectModel;
using System.Linq;

namespace Prison.Data
{
    internal static class TableHelper
    {
        public static ObservableCollection<T> Sort<T>(ObservableCollection<T> sorter, string propertyName)
        {
            var s = sorter.OrderBy(a => a.GetType().GetProperty(propertyName).GetValue(a, null));
            sorter = new ObservableCollection<T>();
            foreach (var item in s)
                sorter.Add(item);
            return sorter;
        }

        //public ObservableCollection<T> Filter<T>(ObservableCollection<T> sorter, string propertyName, object value)
        //{
        //    var d = from sort in sorter where sort.GetType().GetProperty(propertyName).GetValue(sort) == value select sort;
        //    var s = sorter.Where(a =>
        //    a.GetType().GetProperty(propertyName).GetValue(a) == value &&
        //    value == value
        //    );
        //    sorter = new ObservableCollection<T>();
        //    foreach (var item in s)
        //        sorter.Add(item);
        //    return sorter;
        //}
    }
}
