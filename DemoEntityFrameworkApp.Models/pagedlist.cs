using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Models
{
    public class pagedlist
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string search { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }

    }
}
