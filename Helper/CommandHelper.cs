using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KKIHub.ContentSync.Web.Helper
{
    public static class CommandHelper
    {
        public static void ExcecuteScript(string filePath, string syncId, string assetList)
        {
            try
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{filePath}\" {syncId}",
                    UseShellExecute = true
                };

                startInfo.Arguments = assetList != null
                   ? $"-NoProfile -ExecutionPolicy unrestricted -File \"{filePath}\" {syncId} {assetList}"
                   : startInfo.Arguments;

                Process.Start(startInfo);
            }

            catch (Exception err)
            {
                Debug.WriteLine(err);
            }
        }

        public static string ExcecuteScriptOutput(string filePath, string syncId, string assetList)
        {
            var finalOutputMessage = string.Empty;
            try
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -File \"{filePath}\" {syncId}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                startInfo.Arguments = assetList != null
                  ? $"-NoProfile -ExecutionPolicy unrestricted -File \"{filePath}\" {syncId} {assetList}"
                  : startInfo.Arguments;

                var process = Process.Start(startInfo);

                while (!process.StandardOutput.EndOfStream)
                {
                    finalOutputMessage += process.StandardOutput.ReadLine() + Environment.NewLine;
                }
            }

            catch (Exception err)
            {
                Debug.WriteLine(err);
                finalOutputMessage = err.ToString();
            }

            return finalOutputMessage;
        }
    }
}
