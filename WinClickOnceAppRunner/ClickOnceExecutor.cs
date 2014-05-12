using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ClickOnceLocalManager;

namespace WinClickOnceAppRunner
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
               };
         process.Start();
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