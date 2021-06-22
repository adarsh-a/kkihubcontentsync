using System.Collections.Generic;

namespace KKIHub.ContentSync.Web.Models
{
    public class ContentModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LibraryId { get; set; }
        public string LibraryName { get; set; }
        public string Filename { get; set; }
        public List<AssetModel> Assets { get; set; }
    }
}
