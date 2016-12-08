using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static string _projPath;
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
            //TODO: making many api calls
            return Convert.ToInt32(itemHistory.Count);
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
                            try
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
                            catch (AggregateException)
                            {
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

        public static List<TfsItemViewModel> SetPathParams(string projPath, string tfsPath)
        {
            _projPath = projPath;
            _tfsPath = tfsPath;

            var files = new List<string>();

            if (File.Exists(projPath) && projPath.EndsWith(".csproj"))
            {
                GenerateCodeMetrics(projPath);
            }

            if (Directory.Exists(projPath))
            {
                //TODO: limited file types to cs
                files = Directory.GetFiles(projPath, "*.cs", SearchOption.AllDirectories).ToList();
                //files = Directory.GetFiles(Path.GetDirectoryName(projPath), "*.cs", SearchOption.AllDirectories).ToList();
            }

            data.Clear();
            Parallel.ForEach(files, file =>
            {
                var tfsFile = tfsPath + file.Replace(projPath, "").Replace(@"\", "/");
                var result = TfsHelper.GetItemHistory(tfsFile);
                if (result != null)
                {
                    data.Add(new TfsItemViewModel()
                    {
                        FullPath = file,
                        Name = Path.GetFileName(file),
                        BugCount = GetBugCount(result),                        
                        Score = GetCodeMetricsScore(file),
                        Size = new FileInfo(file).Length
                    });
                }
            });

            return data;
        }

        private static int GetCodeMetricsScore(string file)
        {
            //TODO: 
            var r = new Random();
            return r.Next(50,90);
        }

        private static void GenerateCodeMetrics(string projFile)
        {
            return;
            var s = @"""C:\Program Files (x86)\Microsoft Visual Studio 14.0\Team Tools\Static Analysis Tools\FxCop\metrics.exe"" /out:temp.xml /file:""" + projFile + @"""";
            Process.Start("CMD.exe", s);
        }

        public static List<TfsItemViewModel> GetData()
        {
            return data;
        }
    }
}
