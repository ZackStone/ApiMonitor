using System.Collections.Generic;
using System.Net.Http;

namespace ApiMonitor
{
    public class RequestConfig
    {
        public string Id { get; set; }
        public string Uri { get; set; }
        public string Method { get; set; }
        public Auth Auth { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }

    public class Auth
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
