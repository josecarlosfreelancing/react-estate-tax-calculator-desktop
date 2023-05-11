using System.IO;
using EstateView.Core.Model;
using Newtonsoft.Json;

namespace EstateView.Utilities
{
    public static class SaveLoadHelper
    {
        public static void Save(EstateProjectionOptions options, string filename)
        {
            string json = JsonConvert.SerializeObject(options, Formatting.Indented);
            File.WriteAllText(filename, json);
        }

        public static EstateProjectionOptions Load(string filename)
        {
            string json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<EstateProjectionOptions>(json);
        }
    }
}
