using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Models
{
    public class USP_GET_ALL_PERSONS
    {
        public int PersonID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string HireDate { get; set; }
        public string EnrollmentDate { get; set; }
        public string Salary { get; set; }
        public int totalcount { get; set; }
    }
}
