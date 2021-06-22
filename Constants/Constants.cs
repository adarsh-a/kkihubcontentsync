using System.Collections.Generic;

namespace KKIHub.ContentSync.Web.Constants
{
    public static class Constants
    {
        public static Dictionary<string, string> HubToApi = new Dictionary<string, string>
        {
            { "dev", "0cfd35055f2b4d22bca268ef6aafa477" },
            { "uat", "e731bd7f39b8494aa2af7cd0fbc8dbe4" },
            { "non-prod-8", "312b35e554ac4a408ca05b3a1d982f73" },
            { "hangar-test", "48b3ec34dd1943e799dbf8cc318a03a4" }
        };

        public static Dictionary<string, string> HubNameToId = new Dictionary<string, string>
        {
            { "dev", "5892a5bb-1872-4776-8e96-7fdacade46a9" },
            { "uat", "a590dc33-59b3-4b9b-b542-612ac16a7b39" },
            { "non-prod-8", "37dd7bf6-5628-4aac-8464-f4894ddfb8c4" },
            { "hangar-test", "70aea51c-4e79-4d9e-bbf5-4a2c21a23a3c" }
        };

        public static Dictionary<string, string> LibraryIdMap = new Dictionary<string, string>
        {
            { "040d342f-c78d-44c2-872c-b5f0acd07d6b","Deutschland" },
            { "4838a63c-9cd1-4d4e-a28e-3db6fda6ac38" ,"Ireland"},
            { "95cc85ac-1669-4088-bcd2-c8fff18bd617" ,"France"},
            { "70aea51c-4e79-4d9e-bbf5-4a2c21a23a3c" ,"Italia"},
            { "0a861b00-ad92-4d19-b851-443f11c4a683","Netherlands" },
            { "c8d215b7-2b92-42c7-b8bf-22ce07727798","Norge"},
            { "77d05f48-8ade-4160-be16-09a97605c322","United Kingdom"},
            { "064d499c-fad8-46bf-96d9-99a1c323743f","España"},
            { "default","default" }
        };

        public struct Endpoints
        {
            public static string Base = "https://content-eu-1.content-cms.com/api/{hubId}/";
            public static string FetchContentDateRange = "authoring/v1/content/views/by-modified";
            public static string FetchContentById = "authoring/v1/content";
            public static string FetchAssetsById = "authoring/v1/assets";
            public static string FetchType = "authoring/v1/types";

        }

        public struct Path
        {
            public static string ArtifactPath = @"Artifacts\";
            public static string WchtoolsPath = @"wchtools_non-prod.ps1";
        }
    }
}
