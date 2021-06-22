using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KKIHub.ContentSync.Web.Models
{
    public class SyncModel
    {
        public List<HubInfo> HubInfo { get; set; }

        public SyncModel()
        {
            HubInfo = new List<HubInfo>();
        }
    }
}