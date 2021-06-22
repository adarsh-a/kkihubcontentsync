using System.Collections.Generic;

namespace KKIHub.ContentSync.Web.Models
{
    public class PushParams
    {
        public string SyncId { get; set; }

        public string TargetHub { get; set; }

        public string SourceHub { get; set; }

        public List<ContentDetails> ContentDetails { get; set; }
    }
}
