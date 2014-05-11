using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ClickOnceLocalManager
{
   public class ClickOnceDownloader
   {
      readonly string localRootPath;

      public ClickOnceDownloader(string folder, Uri applicationUri)
      {
         localRootPath = folder;
         ApplicationUrl = applicationUri.ToString();
      }
      public string ApplicationUrl { get; private set; }
      public ClickOnceManifest ApplicationManifest { get; private set; }

      public void Download()
      {
         var applicationManifestXml = XElement.Load(ApplicationUrl);
         var absoluteCodebase = 
            new Uri(ManifestParser.GetUriWithoutFile(ApplicationUrl), 
               ParseCodebase(applicationManifestXml));
         ApplicationManifest = LoadManifest(applicationManifestXml, absoluteCodebase);
         DownloadFiles(ApplicationManifest);
      }

      ClickOnceManifest LoadManifest(XElement applicationManifestXml, Uri absoluteCodebase)
      {
         ClickOnceManifest manifest = ManifestParser.DownloadAndParseManifest(absoluteCodebase);
         manifest.Version = ParseVersion(applicationManifestXml);
         manifest.LocalPath = GetVersionPath(manifest.Version);
         return manifest;
      }

      string GetVersionPath(Version version)
      {
         return Path.Combine(localRootPath, version.ToString());
      }

      static Version ParseVersion(XElement applicationManifest)
      {
         return new Version(
            applicationManifest.Elements(XName.Get("assemblyIdentity", "urn:schemas-microsoft-com:asm.v1")).First()
               .Attribute(XName.Get("version")).Value);
      }

      static string ParseCodebase(XElement applicationManifest)
      {
         return applicationManifest.Elements(
            XName.Get("dependency", "urn:schemas-microsoft-com:asm.v2")).First()
            .Elements(XName.Get("dependentAssembly", "urn:schemas-microsoft-com:asm.v2")).First().Attribute(XName.Get("codebase")).Value;
      }

      static void DownloadFiles(ClickOnceManifest manifest)
      {
         if (!Directory.Exists(manifest.LocalPath))
            Directory.CreateDirectory(manifest.LocalPath);
         WebClient webClient = new WebClient();
         foreach (string path2 in manifest.Files)
         {
            Uri address = new Uri(manifest.ApplicationBaseUri, path2.Replace('\\', '/') + ".deploy");
            string str = Path.Combine(manifest.LocalPath, path2);
            string path = Regex.Replace(str, "(?<!:)\\\\[^\\\\]+$", "");
            if (!Directory.Exists(path))
               Directory.CreateDirectory(path);
            webClient.DownloadFile(address, str);
         }
      }
   }
}
