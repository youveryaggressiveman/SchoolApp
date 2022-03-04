using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core
{
    public static class ServerManager
    {
        private static readonly string _server = "http://localhost:38628/swagger/";

        public static string GetServer() => _server;
    }
}
