using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Prison.Data
{
    internal static class TableHelper
    {
        public static ObservableCollection<T> Sort<T>(ObservableCollection<T> sorter, string propertyName, bool isAscending)
        {
            IOrderedEnumerable<T> sort;
            if (isAscending)
                sort = sorter.OrderBy(a => a.GetType().GetProperty(propertyName).GetValue(a, null));
            else
                sort = sorter.OrderByDescending(a => a.GetType().GetProperty(propertyName).GetValue(a, null));

            sorter = new ObservableCollection<T>();
            foreach (var item in sort)
                sorter.Add(item);
            return sorter;
        }

        private static string jsonToCSV(string jsonContent)
        {
            var expandos = JsonConvert.DeserializeObject<ExpandoObject[]>(jsonContent);
            CsvConfiguration configuration = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture);
            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer,configuration))
                {
                    csv.WriteRecordsAsync(expandos as IEnumerable<dynamic>);
                }

                return writer.ToString();
            }
        }

        public static void Export<T>(ObservableCollection<T> table, string tableName)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Выбор папки";
            dlg.IsFolderPicker = true;

            var cvs = jsonToCSV(JsonConvert.SerializeObject(table));
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                using FileStream file = File.Create($@"{dlg.FileName}\{nameof(tableName)}.txt");
                byte[] title = new UTF8Encoding(true).GetBytes(cvs);
                file.Write(title);
            }
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
