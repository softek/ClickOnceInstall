using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ClickOnceLocalManager
{
   public static class ClickOnceExecutor
   {
      public static void Start(this ClickOnceManifest manifest, string[] args)
      {
         string fileName = Path.Combine(manifest.LocalPath, manifest.CommandLine);
         string arguments = string.Join(" ", args.Select(s => '"' + s + '"').ToArray());
         Process process =
            new Process()
               {
                  StartInfo = CreateProcessStartInfo(fileName, arguments, manifest.LocalPath),
                  EnableRaisingEvents = true
               };
         process.OutputDataReceived += (DataReceivedEventHandler)((s, dea) => Console.Error.WriteLine(dea.Data));
         process.ErrorDataReceived += (DataReceivedEventHandler)((s, dea) => Console.WriteLine(dea.Data));
         Console.WriteLine("Starting app: " + fileName);
         process.Start();
         process.BeginOutputReadLine();
         process.BeginErrorReadLine();
         Console.WriteLine("Started");
         process.WaitForExit();
      }

      static ProcessStartInfo CreateProcessStartInfo(string fileName, string arguments, string workingDirectory)
      {
         return
            new ProcessStartInfo(fileName, arguments)
            {
               RedirectStandardOutput = true,
               RedirectStandardError = true,
               UseShellExecute = false,
               WorkingDirectory = workingDirectory
            };
      }
   }
}