using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speech
{
    public class UnaEnumerator : ISpeechEnumerator
    {
        public const string EngineName = "AITalk";
        public const string LibraryName = "音街ウナTalk Ex";

        public SpeechEngineInfo[] GetSpeechEngineInfo()
        {
            var info = new List<SpeechEngineInfo>();
            var path = GetInstalledPath();
            if (!File.Exists(path))
            {
                return info.ToArray();
            }
            info.Add(new SpeechEngineInfo { EngineName = EngineName, EnginePath = path, LibraryName = LibraryName });
            return info.ToArray();
        }

        private string GetInstalledPath()
        {
            string path = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store\";
            var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(path, false);
            if (key != null)
            {
                foreach (string name in key.GetValueNames())
                {
                    if (name != null && name.ToString().EndsWith("OtomachiUnaTalkEx.exe"))
                    {
                        return name;
                    }
                }
            }
            return "";
        }
    }
}
