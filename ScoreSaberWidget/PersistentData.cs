using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ScoreSaberWidget
{
    public class PersistentData
    {
        private const string Preferences = "preferences.json";
        public long UserId { get; set; }
        public PersistentData(long userId)
        {
            UserId = userId;
        }
        public void Save(Context context)
        {
            var stream = context.OpenFileOutput(Preferences, FileCreationMode.Private);
            using var textWriter = new StreamWriter(stream);
            textWriter.Write(JsonConvert.SerializeObject(this));
        }
        public static PersistentData Load(Context context)
        {
            try
            {
                var str = File.ReadAllText(Path.Combine(context.FilesDir.AbsolutePath, Preferences));
                return JsonConvert.DeserializeObject<PersistentData>(str);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}