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
            return (ObservableCollection<T>)JsonConvert.DeserializeObject(responseBody, typeof(ObservableCollection<T>));
        }

        public static async Task<T> Authorization<T>(object value)
        {
            string responseBody = await PostRequest(value);
            return (T)JsonConvert.DeserializeObject(responseBody, typeof(T));
        }
    }
}
