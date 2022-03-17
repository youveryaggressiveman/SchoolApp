using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core
{
    public static class ServerManager
    {
        private static readonly string s_server = "http://localhost:23329/swagger/";

        public static string GetServer() => s_server;

        public static async Task<bool> GetAccessibleServer()
        {

            var request = WebRequest.Create(s_server);
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
