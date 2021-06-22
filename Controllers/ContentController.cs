using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KKIHub.ContentSync.Web.Helper;
using KKIHub.ContentSync.Web.Models;
using KKIHub.ContentSync.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KKIHub.ContentSync.Web.Controllers
{
    [Route("api/[controller]")]

    public class ContentController : Controller
    {
        private IContentService ContentService { get; set; }

        public ContentController(IContentService contentService)
        {
            this.ContentService = contentService;
        }


        [HttpGet]
        [Route("SyncComplete")]
        public async Task<ActionResult> SyncContentAsync(int days, string sourceHub, string targetHub, string syncId)
        {
            var content = await ContentService.FetchContentAsync(syncId, days, sourceHub, false, false);

            return Json(content);
        }


        [HttpGet]
        [Route("SyncRecursive")]
        public async Task<ActionResult> SyncContentRecursive(int days, string sourceHub, string targetHub, string syncId)
        {
            var content = await ContentService.FetchContentAsync(syncId, days, sourceHub, true, false);

            return Json(content);
        }

        [HttpGet]
        [Route("SyncContentUpdated")]
        public async Task<ActionResult> SyncContentUpdated(int days, string sourceHub, string targetHub, string syncId)
        {
            var content = await ContentService.FetchContentAsync(syncId, days, sourceHub, true, true);

            return Json(content);
        }

        [HttpGet]
        [Route("ContentByLibrary")]
        public async Task<ActionResult> ContentByLibrary(string sourceHub, string libraryId, string syncId)
        {
            var content = await ContentService.FetchContentByLibrary(syncId, sourceHub, libraryId);

            return Json(content);
        }

        [HttpGet]
        [Route("PushContent")]
        public JsonResult PushContent(string filepaths, string syncId)
        {
            string message = string.Empty;
            if (!string.IsNullOrEmpty(filepaths))
            {
                try
                {
                    //Dictionary<string, string> pushData = JsonConvert.DeserializeObject<Dictionary<string, string>>(pushParams);
                    var files = filepaths;
                    //var targetHub = pushData["targethub"];

                    if (!string.IsNullOrEmpty(files))
                    {
                        var filePath = files.Split('|').ToList();

                        var contentList = JsonCreator.ListContent(syncId, "content");
                        if (contentList.Any() && filePath.Any())
                        {
                            var itemsToDelete = contentList.Except(filePath).ToList();
                            if (itemsToDelete.Any())
                            {
                                var flag = JsonCreator.Delete(syncId, "content", itemsToDelete);
                            }
                            //message = CommandHelper.ExcecuteScriptOutput(Path.Combine(Environment.CurrentDirectory, Constants.Constants.Path.WchtoolsPath));
                            var assets = ContentService.FetchAssetsList().ToList();

                            var assetString = string.Empty;
                            foreach (var asset in assets)
                            {
                                assetString = string.Join("|", asset.Path);
                            }


                            assetString = assetString.TrimEnd('|');
                            CommandHelper.ExcecuteScript(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Constants.Path.WchtoolsPath), syncId, assetString);
                        }


                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError($"Error pushing content at {ex.Message} ");
                    message = ex.Message;
                }
            }

            //var content = await ContentService.FetchContentByLibrary(sourceHub, libraryId);

            return Json(message);
        }

        [HttpPost]
        [Route("PushContentv2")]
        public JsonResult PushContentv2(string pushParams)
        {
            string msg = string.Empty;
            Console.WriteLine("Starting push command with" + pushParams);
            var pathDynamic = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(pushParams))
            {
                PushParams pushData = JsonConvert.DeserializeObject<PushParams>(pushParams);
                if (pushData != null)
                {
                    Console.WriteLine("pushData success");
                    var hubId = Constants.Constants.HubNameToId[pushData.SourceHub];
                    var syncId = pushData.SyncId;
                    if (pushData.ContentDetails != null && pushData.ContentDetails.Any())
                    {
                        DeleteUnnecessaryItems(pushData.ContentDetails, syncId);
                        Console.WriteLine("Delete Ended");
                    }

                    var assetsFiles = pushData.ContentDetails.Select(i => i.Assets).ToList();
                    if (assetsFiles != null && assetsFiles.Any())
                    {
                        foreach (var assetFile in assetsFiles)
                        {
                            if (assetFile != null && assetFile.Any())
                            {
                                foreach (var asset in assetFile)
                                {
                                    if (!string.IsNullOrEmpty(asset))
                                    {
                                        var assetMsg = PullAssets(hubId, asset, syncId);
                                    }
                                }
                            }
                        }
                    }

                    //push asset
                    var innerMsg = PushCommand(pushData.TargetHub, syncId, "pushasset");

                    //push content
                    var innerMsg2 = PushCommand(pushData.TargetHub, syncId, "pushcontent");

                    //DeleteAllFiles(syncId);

                    /*  var targetHub = Constants.Constants.HubNameToId[pushData.TargetHub];
                      var path = @"C:\inetpub\wwwroot\KKIHUB.ContentSync.Web";
                      string initCommand = $"/C npm run push -- --syncid {syncId} --hubid {targetHub}";

                      try
                      {
                          var p = new Process
                          {
                              StartInfo =
                               {
                                   FileName = "cmd.exe",
                                   WorkingDirectory = path,
                                   Arguments = initCommand,
                                   UseShellExecute = false,
                                   RedirectStandardOutput = true,
                                   Verb= "runas"
                              }
                          };
                          p.Start();
                          msg = p.StandardOutput.ReadToEnd();


                          DeleteAllFiles(syncId);
                      }
                      catch (Exception err)
                      {
                          msg = $"{err.Message} at {err.StackTrace}";

                      }*/
                }
            }
            return Json(msg);
        }


        public string PushCommand(string targetHub, string syncId, string jobName)
        {
            var targetHubId = Constants.Constants.HubNameToId[targetHub];
            var path = AppDomain.CurrentDomain.BaseDirectory;
            //var path = HttpRuntime.AppDomainAppPath;
            string initCommand = $"/C npm run {jobName} -- --syncid {syncId} --hubid {targetHubId}";
            string msg = string.Empty;
            try
            {
                var p = new Process
                {
                    StartInfo =
                             {
                                 FileName = "cmd.exe",
                                 WorkingDirectory = path,
                                 Arguments = initCommand,
                                 UseShellExecute = false,
                                 RedirectStandardOutput = true,
                                 Verb= "runas"
                            }
                };
                p.Start();
                msg = p.StandardOutput.ReadToEnd();
            }
            catch (Exception err)
            {
                msg = $"{err.Message} at {err.StackTrace}";
                Console.WriteLine(msg + $" at {jobName}");

            }
            return msg;
        }

        private string PullAssets(string sourceHub, string assetPath, string syncId)
        {
            //var path = HttpRuntime.AppDomainAppPath;
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"Path is {path}");

            try
            {

                var p2 = new Process
                {
                    StartInfo =
                     {
                         FileName = "cmd.exe",
                         WorkingDirectory = path,
                         Arguments = "docker ",
                         UseShellExecute = false,
                         RedirectStandardOutput = true,
                         Verb= "runas"
                    }
                };
                p2.Start();
                string msg1 = p2.StandardOutput.ReadToEnd();
            }
            catch (Exception err)
            {
                string msg2 = $"{err.Message} {err.InnerException}  at {err.StackTrace}";
                Console.WriteLine(msg2 + " at first command");
            }


            string initCommand = $"/C npm run pullasset -- --path {assetPath}  --syncid {syncId} --hubid {sourceHub}";
            string msg = string.Empty;
            try
            {
                var p = new Process
                {
                    StartInfo =
                     {
                         FileName = "cmd.exe",
                         WorkingDirectory = path,
                         Arguments = initCommand,
                         UseShellExecute = false,
                         RedirectStandardOutput = true,
                         Verb= "runas"
                    }
                };
                p.Start();
                msg = p.StandardOutput.ReadToEnd();
            }

            catch (Exception err)
            {
                msg = $"{err.Message} at {err.StackTrace}";
                Console.WriteLine(msg + " at Pull Assets");
            }

            return msg;
        }


        private void DeleteUnnecessaryItems(List<ContentDetails> contents, string syncId)
        {
            var contentList = JsonCreator.ListContent(syncId, "content");
            var filePaths = contents.Select(i => i.Item).ToList();

            var itemsToDelete = contentList.Except(filePaths).ToList();
            if (itemsToDelete.Any())
            {
                var flag = JsonCreator.Delete(syncId, "content", itemsToDelete);
            }
        }

        private void DeleteAllFiles(string syncId)
        {
            var flag = JsonCreator.DeleteAll(syncId);

        }

        private void ExecuteCommand()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, Constants.Constants.Path.ArtifactPath, "wchtools_non-prod.cmd");
            //System.Diagnostics.Process.Start(filePath);


            var process = new Process();
            //var startinfo = new ProcessStartInfo(filePath, "\"1st_arg\" \"2nd_arg\" \"3rd_arg\"");
            var startinfo = new ProcessStartInfo("cmd.exe", "/c " + filePath);
            startinfo.RedirectStandardOutput = true;
            startinfo.UseShellExecute = false;
            process.StartInfo = startinfo;
            process.OutputDataReceived += (sender, argsx) => Console.WriteLine(argsx.Data); // do whatever processing you need to do in this handler
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }


        static void ExecuteCommandInApp(string command, string workingDirectory)
        {
            int exitCode;

            var nodePath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Microsoft\VisualStudio\NodeJs\win-x64\node.exe";
            string initCommand = "wchtools init --url https://content-eu-1.content-cms.com/api/37dd7bf6-5628-4aac-8464-f4894ddfb8c4 --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU";

            var processInfo = new ProcessStartInfo(nodePath, "/c " + initCommand);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.WorkingDirectory = workingDirectory;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }



        private void SyncContent()
        {

            var filePath = Path.Combine(Environment.CurrentDirectory, Constants.Constants.Path.ArtifactPath);

            //string installWchtools = "call npx -i -g --production --no-optional wchtools-cli";
            // ExecuteCommandInApp(installWchtools, filePath);
            //string initCommand = "npx wchtools init --url https://content-eu-1.content-cms.com/api/37dd7bf6-5628-4aac-8464-f4894ddfb8c4 --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU";
            //ExecuteCommandInApp(initCommand, filePath);

            var commandFile = string.Concat(filePath, "wchtools_non-prod.cmd");
            ExecuteCommandInApp(commandFile, filePath);

        }
    }
}