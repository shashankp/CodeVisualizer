using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadTest
{
    public class TfsItem
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public int Score { get; set; }
        public int BugCount { get; set; }
        public string FullPath { get; set; }
        public string TFSPath { get; set; }
    }
}
