using CeVIO.Talk.RemoteService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speech
{
    class CeVIOEnumerator : ISpeechEnumerator
    {
        string[] _name = new string[0];
        public const string EngineName = "CeVIO";

        public SpeechEngineInfo[] GetSpeechEngineInfo()
        {
            List<SpeechEngineInfo> info = new List<SpeechEngineInfo>();
            var dllPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Microsoft.NET\assembly\GAC_32\CeVIO.Talk.RemoteService";
            if (!Directory.Exists(dllPath))
            {
                return info.ToArray();
            }
            _name = Talker.AvailableCasts;
            foreach (var v in _name)
            {
                info.Add(new SpeechEngineInfo { EngineName = EngineName, EnginePath = string.Empty, LibraryName = v });
            }
            return info.ToArray();
        }
    }
}
