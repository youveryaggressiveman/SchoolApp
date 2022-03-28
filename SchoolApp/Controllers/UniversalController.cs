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
            if (await ServerManager.GetAccessibleServer())
            {
                using (HttpClient client = new HttpClient())
                {
                    var stringTask = await client.GetAsync(_server.Replace("/swagger/", "/api/") + listName[0] + "/" + listName[1] + "/" + listName[2] + "?" + listName[2] + "=" + int.Parse(listArgument[0]));

                    if (stringTask.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(await stringTask.Content.ReadAsStreamAsync());

                        return result;
                    }
                }
            }

            throw new OperationCanceledException();
        }

        public async Task<Guid> PostListSomething<C>(C obj, string[] listName, string[] listArgument) where C : class
        {
            if (await ServerManager.GetAccessibleServer())
            {
                var jsonObject = JsonSerializer.Serialize<C>(obj);
                var url = _server.Replace("/swagger/", "/api/") + listName[0] + "/" + listName[1];

                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var stringTask = await client.PostAsync(url, content);

                    if (stringTask.StatusCode == HttpStatusCode.OK)
                    {
                        var result =
                        await JsonSerializer.DeserializeAsync<Guid>(await stringTask.Content.ReadAsStreamAsync());

                        return result;
                    }
                }
            }

            throw new OperationCanceledException();
        }

        public async Task PutAsync<C>(C obj, string[] listName, string[] listArgument)
        {
            if (await ServerManager.GetAccessibleServer())
            {
                using (HttpClient client = new HttpClient())
                {
                    var jsonObject = JsonSerializer.Serialize<C>(obj);
                    var url = _server.Replace("/swagger/", "/api/") + listName[0] + "/" + listName[1] + "/" + listName[2];

                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    await client.PutAsync(url, content);
                }
            }
            else
            {
                throw new OperationCanceledException();
            }
            
        }

        public async Task<IEnumerable<T>> GetList(string[] listName)
        {
            if (await ServerManager.GetAccessibleServer())
            {
                using (HttpClient client = new HttpClient())
                {
                    var stringTask = await client.GetAsync(_server.Replace("/swagger/", "/api/") + listName[0] + "/" + listName[1]);

                    if (stringTask.StatusCode == HttpStatusCode.OK)
                    {
                        var result =
                             await JsonSerializer.DeserializeAsync<IEnumerable<T>>(
                                 await stringTask.Content.ReadAsStreamAsync());

                        return result;
                    }
                }
            }
            throw new OperationCanceledException();
        }
    }
}
