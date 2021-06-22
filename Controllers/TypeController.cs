using System.Threading.Tasks;
using KKIHub.ContentSync.Web.Service;
using Microsoft.AspNetCore.Mvc;

namespace KKIHub.Content.SyncService.Controllers
{
    [Route("api/[controller]")]
    public class TypeController : Controller
    {
        private IContentService ContentService { get; set; }
        public TypeController(IContentService contentService)
        {
            this.ContentService = contentService;
        }


        [HttpGet]
        [Route("Sync")]
        public async Task<ActionResult> SyncContentUpdated(string syncId, string sourceHub)
        {
            var content = await ContentService.FetchTypeAsync(syncId, 0, sourceHub, true, false);

            return Json(content);
        }
    }
}