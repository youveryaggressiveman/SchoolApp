using SchoolApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Core.Builders
{
    public class ScheduleBuilder
    {
        protected Schedule _schedule;

        public ScheduleBuilder() => _schedule = new Schedule();

        public ScheduleBuilder(Schedule schedule) => _schedule = schedule;

        public Schedule Build() => _schedule;

        public ScheduleBuilder WithID(int id)
        {
            _schedule.ID = id;
            return this;
        }

        public ScheduleBuilder WithGroupID(int id)
        {
            _schedule.GroupID = id;
            return this;
        }

        public ScheduleBuilder WithEmployeeID(Guid id)
        {
            _schedule.EmployeeID = id;
            return this;
        }

        public ScheduleBuilder WithDayOfWeekID(int id)
        {
            _schedule.DayOfWeekID = id;
            return this;
        }

        public ScheduleBuilder WithTimeSubjectID(int id)
        {
            _schedule.SubjectID = id;
            return this;
        }

        public ScheduleBuilder WithSubjectID(int id)
        {
            _schedule.SubjectID = id;
            return this;
        }
    }
}
