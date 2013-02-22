using System;
using System.IO;
using System.Linq;
using ClickOnceLocalManager;

namespace ClickOnceAppRunner
{
   class Program
   {
      static void Main(string[] args)
      {
         var applicationUri = new Uri(args[0]);
         RunWithTempFolder(folder => RunFromDirectory(folder, applicationUri, args.Skip(1).ToArray()));
      }

      static void RunWithTempFolder(Action<string> action)
      {
         var localPath = CreateTempFolder();
         try
         {
            action(localPath.FullName);
         }
         finally
         {
            localPath.Delete(true);
         }
      }

      static void RunFromDirectory(string localPath, Uri applicationUri, string [] args)
      {
         var co = new ClickOnceDownloader(localPath, applicationUri);
         co.Download();
         co.ApplicationManifest.Start(args);
      }

      static DirectoryInfo CreateTempFolder()
      {
         string path = Guid.NewGuid().ToString();
         return Directory.CreateDirectory(path);
      }
   }
}
