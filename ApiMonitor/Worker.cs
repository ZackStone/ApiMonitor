using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiMonitor
{
    public static class Worker
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task Work(string path)
        {
            var fileNameList = Directory.GetFiles(path);
            fileNameList = fileNameList.Where(x => x.EndsWith(".json")).ToArray();
            var tasks = fileNameList.Select(x => InternalWork(x));
            await Task.WhenAll(tasks);
        }

        private static async Task InternalWork(string fileName)
        {
            var config = ReadFile(fileName);

            try
            {
                var res = await SendRequest(config);
                var statusCode = res.StatusCode;
                var content = await res.Content.ReadAsStringAsync();

                if (statusCode != HttpStatusCode.OK)
                {
                    string title = $"{config.Id}: { (int)statusCode } {statusCode}";

                    MyToast.Notify(
                        title,
                        DateTime.Now.AddMinutes(10),
                        description: content,
                        onActivated: MyToast.GetOnActivatedEvent(title, content));
                }
            }
            catch (HttpRequestException e) when (e.Message == "No such host is known.") { }
        }

        private static RequestConfig ReadFile(string filePath)
        {
            using var stream = new StreamReader(filePath);
            var str = stream.ReadToEnd();
            var ret = JsonSerializer.Deserialize<RequestConfig>(str);
            return ret;
        }

        private static AuthenticationHeaderValue GetBasicAuthHeader(string usr, string pwd)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{usr}:{pwd}");
            var authString = Convert.ToBase64String(byteArray);
            return new AuthenticationHeaderValue("Basic", authString);
        }

        private static async Task<HttpResponseMessage> SendRequest(RequestConfig config)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(config.Uri),
                Method = new HttpMethod(config.Method)
            };

            foreach (var header in config.Headers ?? new Dictionary<string, string>())
                request.Headers.Add(header.Key, header.Value);

            if (config.Auth != null)
                request.Headers.Authorization = GetBasicAuthHeader(config.Auth.Username, config.Auth.Password);

            var res = await client.SendAsync(request);

            return res;
        }
    }
}
