using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SchoolApp.Core;

namespace SchoolApp.Controllers
{
    public class UniversalController<T> where T : class
    {
        private readonly string _server = ServerManager.GetServer();

        public async Task<IEnumerable<T>> GetListBySomething(string[] listName, string[] listArgument)
        {
            if (await GetAccessibleServer())
            {

            }

            throw new ArgumentNullException();
        }

        public async Task<bool> PostListSomething<C>(C obj, string[] listName, string[] listArgument) where C : class
        {
            if (await GetAccessibleServer())
            {
                var jsonObject = JsonSerializer.Serialize<C>(obj);
                var url = _server.Replace("/swagger/", "/api/");

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(url, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var stringTask = await client.PostAsync(jsonObject, content);

                    if (stringTask.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ArgumentNullException();
                    }

                    var result =
                        await JsonSerializer.DeserializeAsync<bool>(await stringTask.Content.ReadAsStreamAsync());

                    return result;
                }
            }

            throw new ArgumentNullException();
        }

        public async Task<IEnumerable<T>> GetList(string[] listName)
        {
            if (await GetAccessibleServer())
            {
                using (HttpClient client = new HttpClient())
                {
                    var stringTask = await client.GetAsync(_server.Replace("/swagger/", "/api/") + listName[0] + "/");

                    if (stringTask.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ArgumentNullException();
                    }

                    var result =
                        await JsonSerializer.DeserializeAsync<IEnumerable<T>>(
                            await stringTask.Content.ReadAsStreamAsync());

                    return result;
                }
            }

            throw new ArgumentNullException();
        }

        public async Task<bool> GetAccessibleServer()
        {

            var request = WebRequest.Create(_server);
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();
                response.Close();

                return true;
            }
            catch (Exception)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
