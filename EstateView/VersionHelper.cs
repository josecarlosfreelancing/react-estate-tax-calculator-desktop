using System.Linq;
using System.Reflection;

namespace EstateView
{
    public static class VersionHelper
    {
        public static string Version
        {
            get
            {
                return
                    Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), inherit: false)
                    .Cast<AssemblyInformationalVersionAttribute>()
                    .Single()
                    .InformationalVersion;
            }
        }
    }
}