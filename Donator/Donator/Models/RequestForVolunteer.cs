using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Models
{
    public class RequestForVolunteer
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int NPOId { get; set; }
        public virtual NPO NPO { get; set; }
        public int Status { get; set; }
        public bool WaiverRequired { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string UpdatedByUserId { get; set; }
        public virtual User UpdatedByUser { get; set; }

    }
}
