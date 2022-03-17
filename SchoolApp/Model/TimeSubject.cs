using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolApp.Model
{
    public class TimeSubject
    {
        public int ID { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        [JsonIgnore]
        public string Time => Begin.Hour.ToString() + ":" + Begin.Minute.ToString() + " - " + End.Hour.ToString() +
                              ":" + End.Minute.ToString();
    }
}
