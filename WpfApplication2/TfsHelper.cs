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
        private static string _slnPath;
        private static string _tfsPath;
        private static List<TfsItemViewModel> data = new List<TfsItemViewModel>(); 

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

        public static Changeset GetChangesetWithLinks(Changeset changeset)
        {
            try
            {
                var url = new Uri(changeset.Url);
                //Use Windows credentials
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true
                }))
                {
                    var result = httpClient.GetStringAsync(url).Result;
                    var changesetWithLinks = JsonConvert.DeserializeObject<Changeset>(result);
                    return changesetWithLinks;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static int GetBugCount(ItemHistory itemHistory)
        {
            int bugCount = 0;
            try
            {

                Parallel.ForEach(itemHistory.Value, changeset =>
                {
                    {
                        Changeset cs = GetChangesetWithLinks(changeset);

                        var url = cs._Links.WorkItems.href;
                        using (var httpClient = new HttpClient(new HttpClientHandler
                        {
                            UseDefaultCredentials = true
                        }))
                        {
                            var result = httpClient.GetStringAsync(url).Result;
                            var associatedWorkItems = JsonConvert.DeserializeObject<AssociatedWorkItems>(result);

                            foreach (WorkItem workitem in associatedWorkItems.Value)
                            {
                                if (workitem.WorkItemtype == "Bug")
                                {
                                    bugCount += 1;
                                }
                            }

                        }
                    }

                });
               
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bugCount;

        }

        public static List<TfsItemViewModel> SetPathParams(string slnPath, string tfsPath)
        {
            _slnPath = slnPath;
            _tfsPath = tfsPath;

            var files = new List<string>();
            if (Directory.Exists(slnPath))
            {
                //TODO: limited file types to cs
                files = Directory.GetFiles(slnPath, "*.cs", SearchOption.AllDirectories).ToList();
            }
            else if (File.Exists(slnPath))
            {
                files.Add(slnPath);
            }

            data.Clear();
            Parallel.ForEach(files, file =>
            {
                var tfsFile = tfsPath + file.Replace(slnPath, "").Replace(@"\", "/");
                var result = TfsHelper.GetItemHistory(tfsFile);
                if (result != null)
                {
                    data.Add(new TfsItemViewModel()
                    {
                        FullPath = file,
                        Name = Path.GetFileName(file),
                        BugCount = GetBugCount(result),
                        Score = 10,
                        Size = new FileInfo(file).Length
                    });
                }
            });

            return data;
        }

        public static List<TfsItemViewModel> GetData()
        {
            return data;
        }
    }
}
