using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speech
{
    public class SpeechController
    {
        public static SpeechEngineInfo[] GetAllSpeechEngine()
        {
            List<SpeechEngineInfo> info = new List<SpeechEngineInfo>();

            // VOICEROID+ を列挙
            var voiceroidPlus = new VoiceroidPlusEnumerator().GetSpeechEngineInfo();
            if(voiceroidPlus.Length > 0)
            {
                info.AddRange(voiceroidPlus);
            }

            // VOICEROID2 を列挙
            var voiceroid2 = new Voiceroid2Enumerator().GetSpeechEngineInfo();
            if(voiceroid2.Length > 0)
            {
                info.AddRange(voiceroid2);
            }

            var una = new UnaEnumerator().GetSpeechEngineInfo();
            if(una.Length > 0)
            {
                info.AddRange(una);
            }

            //CeVIO を列挙
            var cevio = new CeVIOEnumerator().GetSpeechEngineInfo();
            if(cevio.Length > 0)
            {
                info.AddRange(cevio);
            }

            // SAPI5 を列挙
            var sapi5 = new SAPI5Enumerator();
            info.AddRange(sapi5.GetSpeechEngineInfo());

            return info.ToArray();
        }

        public static ISpeechEngine GetInstance(string libraryName)
        {
            var info = GetAllSpeechEngine();
            foreach(var e in info)
            {
                if(e.LibraryName == libraryName)
                {
                    return GetInstance(e);
                }
            }
            return null;
        }

        public static ISpeechEngine GetInstance(SpeechEngineInfo info)
        {
            switch (info.EngineName)
            {
                case VoiceroidPlusEnumerator.EngineName:
                    return new VoiceroidPlusController(info);
                case Voiceroid2Enumerator.EngineName:
                    return new Voiceroid2Controller(info);
                case UnaEnumerator.EngineName:
                    return new UnaController(info);
                case CeVIOEnumerator.EngineName:
                    return new CeVIOController(info);
                case SAPI5Enumerator.EngineName:
                    return new SAPI5Controller(info);
                default:
                    break;
            }
            return null;
        }
    }
}
