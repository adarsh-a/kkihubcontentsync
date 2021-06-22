using KKIHub.ContentSync.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KKIHub.ContentSync.Web.Service
{
    public interface IAcousticService
    {
        Task<List<ContentModel>> FetchArtifactForDateRangeAsync(string syncId, int days, string hub, bool recursive, bool onlyUpdated);

        Task<List<string>> FetchTypeAsync(string syncId, int days, string hub, bool recursive, bool onlyUpdated);

        Task<List<ContentModel>> FetchContentByLibrary(string syncId, string hub, string libraryId);

        List<AssetModel> FetchAssetsList();
    }
}
