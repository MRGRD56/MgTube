using System;
using System.Collections.Generic;
using System.Linq;

namespace MgUrlParser
{
    public class MgUrl
    {
        public MgUrl(string url)
        {
            Url = url;
        }

        private string _url;

        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                Parse();
            }
        }

        public string Protocol { get; private set; }
        public string Hostname { get; private set; }
        public string File { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public string Fragment { get; private set; }


        private void Parse()
        {
            // Url = "https://ru.wikipedia.org/somedirone/somedirtwo/page.html?arg1=string%201337&name=John&age=18#1488228"
            // Protocol = "https"
            // Hostname = "ru.wikipedia.org"
            // File = somedirone/somedirtwo/page.html
            // Parameters = [ ["arg1", "string 1337"], ["name", "John"], ["age", "18"] ]
            // Fragment = "1488228"

            // https://en.russia.ru/path/index.php?a=123&b=234#111

            Url = Url.Replace("%20", " ");

            var urlNoProtocol = "";
            if (Url.Contains("://"))
            {
                var split = Url.Split("://");
                Protocol = split[0];
                urlNoProtocol = split[1];
            }
            else
            {
                urlNoProtocol = Url;
            }

            var paramsSplit = urlNoProtocol.Split("?");
            var slashSplit = paramsSplit[0].Split("/");
            Hostname = slashSplit[0];
            var file = "";
            slashSplit.Skip(1).ToList().ForEach(x => file += $"{x}/");
            file.TrimEnd('/');
            File = file;
            var paramsHashSplit = paramsSplit[1].Split("#");
            var paramsPairs = paramsHashSplit[0].Split("&");
            foreach (var paramsPair in paramsPairs)
            {
                var split = paramsPair.Split("=");
                Parameters.Add(split[0], split[1]);
            }

            Fragment = paramsHashSplit[1];
        }
    }
}
