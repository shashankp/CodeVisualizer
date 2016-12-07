using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfApplication2.Models;

namespace WpfApplication2
{
    public static class TfsHelper
    {
        private static string baseUrl =
            "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection/_apis/tfvc/changesets?searchCriteria.itemPath=";

        public static ItemHistory GetItemHistory(string item)
        {
            try
            {
                var url = new Uri(baseUrl + item);
                //Use Windows credentials
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true
                }))
                {
                    var result = httpClient.GetStringAsync(url).Result;
                    //var j = JObject.Parse(result);
                    var itemHistory = JsonConvert.DeserializeObject<ItemHistory>(result);
                    return itemHistory;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static string GetMapping(string file)
        {
            return file.Replace(@"C:\tfs\Dev\", @"$/WorkflowTools/Dev/").Replace(@"\", "/");
        }

        public static string GetHistory(List<string> files)
        {
            var resultText = "";
            Parallel.ForEach(files, (file) =>
            {
                var tfsFile = TfsHelper.GetMapping(file);
                var result = TfsHelper.GetItemHistory(tfsFile);
                resultText += Path.GetFileName(file) + ", " + result.Count + Environment.NewLine;
            });
            return resultText;
        }
    }
}
