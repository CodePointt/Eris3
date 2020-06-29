using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//just saying at 11:00 I'm going to need to be afk for a while cuz I need to do this meeting thingy Idk when it will end
// Alright
//If it's ok with you can you wait until I get back to do stuff since I feel like it's easier for me to understand stuv
//if i can watch
// Yea sure
//ok im going to add the mute command now
// Sounds good
namespace Eris1.Helpers
{
    public static class JsonManager
    {
        private static void EnsureExists(string filePath)
        {
            var file = Path.Combine(AppContext.BaseDirectory, filePath);
            if (File.Exists(file)) return;
            var path = Path.GetDirectoryName(file);          // Create config directory if doesn't exist.
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.Create(file).Close();
        }
        public static void SaveJson(string filePath, object contents)
        {
            EnsureExists(filePath);
            var file = Path.Combine(AppContext.BaseDirectory, filePath);
            var s = JsonConvert.SerializeObject(contents, Formatting.Indented);
            File.WriteAllText(file, s);
        }

        public static Dictionary<TK, TV> LoadJsonDict<TK, TV>(string filePath)
        {
            var file = Path.Combine(AppContext.BaseDirectory, filePath);
            EnsureExists(file);
            if (string.IsNullOrWhiteSpace(File.ReadAllText(file)))
            {
                var d = new Dictionary<TK, TV>();
                return d;
            }
            var f = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Dictionary<TK, TV>>(f);
        }
    }
}
