using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKInviter
{
    class Proxy
    {
        public string ip;
        public string port;
        public string login;
        public string password;

        public Proxy(string IPAddress, string Port)
        {
            ip = IPAddress;
            port = Port;
        }
        public Proxy(string IPAddress, string Port, string Login, string Password)
        {
            ip = IPAddress;
            port = Port;
            login = Login;
            password = Password;
        }
    }
}
