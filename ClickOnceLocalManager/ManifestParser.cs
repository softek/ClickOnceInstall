using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ClickOnceLocalManager
{
   public static class ManifestParser
   {
      public static ClickOnceManifest DownloadAndParseManifest(Uri uri)
      {
         return Parse(
            XElement.Load(uri.ToString()),
            GetUriWithoutFile(uri.ToString()));
      }

      public static ClickOnceManifest Parse(XElement xml, Uri applicationBaseUri = null)
      {
         ClickOnceManifest manifest = new ClickOnceManifest { ApplicationBaseUri = applicationBaseUri };
         XElement xelement =
            xml.Elements(XName.Get("entryPoint", "urn:schemas-microsoft-com:asm.v2")).First().
               Elements(XName.Get("commandLine", "urn:schemas-microsoft-com:asm.v2")).First();
         manifest.CommandLine = xelement.Attribute(XName.Get("file")).Value;
         manifest.Parameters = xelement.Attribute(XName.Get("parameters")).Value;
         manifest.Files =
            xml.Elements(XName.Get("dependency", "urn:schemas-microsoft-com:asm.v2"))
               .SelectMany((Func<XElement, IEnumerable<XElement>>)
                           (dependency =>
                            dependency.Elements(XName.Get("dependentAssembly", "urn:schemas-microsoft-com:asm.v2"))))
               .Select((Func<XElement, XAttribute>)(depAssem => depAssem.Attribute(XName.Get("codebase"))))
               .Where(attr => attr != null)
               .Select((Func<XAttribute, string>)(attr => attr.Value))
            .Concat(
               xml.Elements(XName.Get("file", "urn:schemas-microsoft-com:asm.v2"))
                  .Select(file => file.Attribute(XName.Get("name")).Value)
               )
            .ToArray();
         return manifest;
      }

      internal static Uri GetUriWithoutFile(string uri)
      {
         return new Uri(Regex.Replace(uri, "(?<=/)[^/]*$", ""));
      }
   }
}