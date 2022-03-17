using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class CurrentSchedule
    {
        public int ID { get; set; }

        public int GroupID { get; set; }
        public Guid EmployeeID { get; set; }
        public int DayOfWeekID { get; set; }
        public int TimeSubjectID { get; set; }
        public int SubjectID { get; set; }

        public virtual Group Group { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DayOfWeek DayOfWeek { get; set; }
        public virtual TimeSubject TimeSubject { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
