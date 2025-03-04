using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3Zip.Entity.Models
{
    public class IISLogEvent
    {

        public string? MessageType { get; set; }
        public string? Owner { get; set; }
        public string? LogGroup { get; set; }
        public string? LogStream { get; set; }
        public string? SubscriptionFilters { get; set; }
        public DateTime? DateTime { get; set; }
        public string? RequestMessage { get; set; }
    }
}
