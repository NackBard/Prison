﻿using Newtonsoft.Json;
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
    }
}
