using SchoolApp.Core;
using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolApp.Controllers
{
    public class AuthViewModelController
    {
        private readonly string _server = ServerManager.GetServer();

        public async Task<bool> GetUserAsync(string email, string password)
        {
            if (await GetAccessibleServer())
            {
                using (HttpClient client = new HttpClient())
                {
                    var stringTask = await client.GetAsync(_server + "api/Users/auth?email=" + email + "&password=" + password);

                    if (stringTask.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return false;
                    }
                    
                    var result = await JsonSerializer.DeserializeAsync<User>(await stringTask.Content.ReadAsStreamAsync());

                    UserSingleton.User = result;

                    return true;
                }
            }

            return false;
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
                return false;
            }
        }
    }
}
