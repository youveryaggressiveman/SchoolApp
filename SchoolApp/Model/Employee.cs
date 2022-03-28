using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class Employee
    {
        public Guid ID { get; set; }
        public int PostID { get; set; }
        public Guid UserID { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string FIO => SecondName + " " + FirstName[0] + "." + (LastName != null ? LastName[0] + "." : "");

    }
}
