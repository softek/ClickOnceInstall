using System;

namespace ClickOnceLocalManager
{
   public class ClickOnceManifest
   {
      public Uri ApplicationBaseUri { get; set; }

      public string[] Files { get; set; }

      public Version Version { get; set; }

      public string CommandLine { get; set; }

      public string Parameters { get; set; }

      public string LocalPath { get; set; }
   }
}