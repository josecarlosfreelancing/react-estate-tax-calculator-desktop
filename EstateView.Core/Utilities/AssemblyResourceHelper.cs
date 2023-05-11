using System.IO;
using System.Reflection;

namespace EstateView.Core.Utilities
{
    public static class AssemblyResourceHelper
    {
        public static string ReadResource(Assembly assembly, string name)
        {
            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
