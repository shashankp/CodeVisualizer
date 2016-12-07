using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadTest
{
    public class TfsItem
    {
        public string TfsPath { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int BugCount { get; set; }
    }
}
