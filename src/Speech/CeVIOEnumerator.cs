using CeVIO.Talk.RemoteService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Speech
{
    public class CeVIOEnumerator : ISpeechEnumerator
    {
        public const string EngineName = "CeVIO";


        public SpeechEngineInfo[] GetSpeechEngineInfo()
        {
            List<SpeechEngineInfo> info = new List<SpeechEngineInfo>();
            var dllPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Microsoft.NET\assembly\GAC_32\CeVIO.Talk.RemoteService";
            //GACにCeVIOのdllが存在するか確認
            if (!Directory.Exists(dllPath))
            {
                return info.ToArray();
            }
            var installedPath = GetInstalledPath();
            //インストールフォルダにCeVIOの実行ファイルが存在するか確認
            if (!File.Exists(installedPath))
            {
                return info.ToArray();
            }

            //Talker.AvailableCasts()はCeVIO起動後でないと使用できない
            //インストールフォルダ\Configuration\VocalSource\Talk\に存在するフォルダ内のcelsysttsinfo.xmlから話者を取得できる
            var exe = "\\CeVIO Creative Studio.exe";
            var path = installedPath.Substring(0, installedPath.Length - exe.Length) + @"\Configuration\VocalSource\Talk\";
            var di = new DirectoryInfo(path);
            var directories = di.EnumerateDirectories();
            var nameList = new List<string>();
            foreach (var dir in directories)
            {
                var fileName = dir.FullName + @"\celsysttsinfo.xml";
                if (File.Exists(fileName))
                {
                    try
                    {
                        var xml = XElement.Load(fileName);
                        nameList.AddRange(from c in xml.Elements("name") select c.Value);
                    }
                    catch
                    {
                    }
                }
            }
            foreach (var v in nameList)
            {
                info.Add(new SpeechEngineInfo { EngineName = EngineName, EnginePath = installedPath, LibraryName = v });
            }
            return info.ToArray();
        }

        private string GetInstalledPath()
        {
            var uninstall_path = @"SOFTWARE\Classes\Installer\Assemblies";
            var result = "";
            var uninstall = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstall_path, false);
            if (uninstall != null)
            {
                foreach (var subKey in uninstall.GetSubKeyNames())
                {
                    var exe = "|CeVIO Creative Studio.exe";
                    if (subKey.EndsWith(exe))
                    {
                        result = subKey.Replace("|", "\\");
                        break;
                    }
                }
            }
            return result;
        }
    }
}
