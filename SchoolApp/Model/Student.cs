using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class Student
    {
        public Guid ID { get; set; }
        public int GroupID { get; set; }
        public Guid UserID { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
