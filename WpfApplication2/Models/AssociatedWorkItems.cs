using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.Models
{
    public class AssociatedWorkItems
    {
        public int Count { get; set; }

        public List<WorkItem> Value { get; set; }
    }
}
