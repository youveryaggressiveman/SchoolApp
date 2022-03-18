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
        public string Begin { get; set; }
        public string End { get; set; }

        [JsonIgnore]
        public string Time => Begin + " - " + End;
    }
}
