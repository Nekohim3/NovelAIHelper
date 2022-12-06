using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace NovelAIHelper.Utils
{
    internal static class Http
    {
        private static RestClient        _client;
        private static RestClientOptions _options;

        static Http()
        {
            _options = new RestClientOptions($"https://danbooru.donmai.us/");
            _client  = new RestClient(_options);
        }

        internal static string GetHtml(string path)
        {
            var req = new RestRequest(path + "/");
            var res = _client.ExecuteAsync(req).GetAwaiter().GetResult();
            return res.Content;
        }
    }
}
