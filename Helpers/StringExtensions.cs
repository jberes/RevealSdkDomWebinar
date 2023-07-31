using System.IO;

namespace Sandbox.Helpers
{
    internal static class StringExtensions
    {
        internal static void Save(this string json, string filePath)
        {
            File.WriteAllText(filePath, json);
        }
    }
}
