using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KKIHub.ContentSync.Web.Helper
{
    public static class JsonCreator
    {
        public static string CreateJsonFile(string syncId, string name, string type, object details)
        {
            var artifactPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constants.Path.ArtifactPath, syncId + "/");
            string path = string.Concat(artifactPath, type);
            Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, name);

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, details.ToString());
                var msg = $"Item with Id {name} created";
                Console.WriteLine(msg);
                return msg;
            }
            return string.Empty;
        }


        public static List<string> ListContent(string syncId, string type)
        {
            var artifactPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constants.Path.ArtifactPath, syncId + "/");
            string path = string.Concat(artifactPath, type);
            var directory = Directory.CreateDirectory(path);

            return directory.GetFiles().Select(i => i.Name).ToList();
        }

        public static bool Delete(string syncId, string type, List<string> itemToDelete)
        {
            var artifactPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constants.Path.ArtifactPath, syncId + "/");
            string path = string.Concat(artifactPath, type);

            foreach (var item in itemToDelete)
            {
                var filePath = Path.Combine(path, item);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            return true;
        }

        public static bool DeleteAll(string syncId)
        {
            var artifactPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constants.Path.ArtifactPath, syncId);
            if (Directory.Exists(artifactPath))
            {
                Directory.Delete(artifactPath);
            }

            return true;
        }
    }
}
