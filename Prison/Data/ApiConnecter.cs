using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Prison.Data
{
    internal class ApiConnector : DataSender
    {
        public static async Task<ObservableCollection<T>> GetAll<T>(string tableName)
        {
            string responseBody = await GetRequest(tableName);
            var df = TableHelper.jsonToCSV(responseBody);
            var ds = TableHelper.CsvToJson(df.Replace("\n","").Split("\r"));
            var zcx = JsonConvert.SerializeObject(ds);
            return (ObservableCollection<T>)JsonConvert.DeserializeObject(responseBody, typeof(ObservableCollection<T>));
        }

        public static async Task<T> Authorization<T>(object value)
        {
            string responseBody = await PostRequest(value);
            return (T)JsonConvert.DeserializeObject(responseBody, typeof(T));
        }

    }
}
