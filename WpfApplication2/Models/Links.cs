using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.Models
{
    public class Links
    {
        public Link Self { get; set; }
        public Link Changes { get; set; }
        public Link WorkItems { get; set; }
        public Link Web { get; set; }
        public Link Author { get; set; }
        public Link CheckedInBy { get; set; }

    }
}
