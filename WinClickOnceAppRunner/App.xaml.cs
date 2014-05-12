using System;
using System.IO;
using System.Linq;
using System.Windows;
using ClickOnceLocalManager;

namespace WinClickOnceAppRunner
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      protected override void OnStartup(StartupEventArgs e)
      {
         base.OnStartup(e);
         var applicationUri = new Uri(e.Args[0]);
         var folder = Path.GetDirectoryName(GetType().Assembly.Location);
         RunFromDirectory(folder, applicationUri, e.Args.Skip(1).ToArray());
         Shutdown();
      }

      static void RunFromDirectory(string localPath, Uri applicationUri, string[] args)
      {
         var co = new ClickOnceDownloader(localPath, applicationUri);
         co.Download();
         co.ApplicationManifest.Start(args);
      }
   }
}
