using System;
using System.IO;
using System.Windows;

namespace WpfTimer
{
    internal static class ResourceWriter
    {
        public static void DumpResource(String resFilename, String outputFile)
        {
            var uri = new Uri("pack://application:,,,/Resources/" + resFilename, UriKind.Absolute);

            using (var ms = new MemoryStream())
            {
                var streamInfo = Application.GetResourceStream(uri);
                streamInfo.Stream.CopyTo(ms);
                File.WriteAllBytes(outputFile, ms.ToArray());
                streamInfo.Stream.Close();
            }
        }
    }
}