using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSample.Models
{
    public class DeviceLog
    {
        public string DeviceId { get; set; }

        public string UserId { get; set; }

        public DateTime LogTime { get; set; }

    }
}