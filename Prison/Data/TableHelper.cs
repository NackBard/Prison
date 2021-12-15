

using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
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

        public static string jsonToCSV(string jsonContent)
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

        public static IEnumerable<JObject> CsvToJson(IEnumerable<string> csvLines)
        {
            var csvLinesList = csvLines.ToList();

            var header = csvLinesList[0].Split(',');
            for (int i = 1; i < csvLinesList.Count; i++)
            {
                var thisLineSplit = csvLinesList[i].Split(',');
                var pairedWithHeader = header.Zip(thisLineSplit, (h, v) => new KeyValuePair<string, string>(h, v));

                yield return new JObject(pairedWithHeader.Select(j => new JProperty(j.Key, j.Value)));
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
