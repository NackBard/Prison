using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace Prison.Data
{
    internal class DataSender
    {
        private const string url = "https://prisonapi.azurewebsites.net/api";

        public static async Task<string> GetRequest(string tableName)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{url}/{tableName}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static async Task<string> PostRequest(string tableName, object value)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{url}/{tableName}", stringContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static async Task<string> PostRequest(object value)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{url}/Workers/authorization", stringContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static async Task<string> PutRequest(string tableName, int id, object value)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{url}/{tableName}/{id}", stringContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static async Task<string> DeleteRequest(string tableName, int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync($"{url}/{tableName}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
