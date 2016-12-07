using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.Models
{
    public class ItemHistory
    {
        public string Count { get; set; }

        public List<Changeset> Value { get; set; } 
    }
}
