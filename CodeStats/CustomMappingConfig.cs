﻿using Kbg.NppPluginNET.PluginInfrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeStats
{
    public class CustomMappingConfig
    {
        internal string ApiKey { get; set; }
        internal string ApiUrl { get; set; }
        internal string Proxy { get; set; }
        internal bool Stats { get; set; }
        internal string Guid { get; set; }
        internal bool Debug { get; set; }

        private readonly string _configFilepath;

        internal CustomMappingConfig()
        {
            _configFilepath = GetConfigFilePath();
            Read();
        }

        internal void Read()
        {
            var ret = new StringBuilder(2083);

            ApiKey = NativeMethods.GetPrivateProfileString("settings", "api_key", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            ApiUrl = NativeMethods.GetPrivateProfileString("settings", "api_url", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            Proxy = NativeMethods.GetPrivateProfileString("settings", "proxy", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            if (NativeMethods.GetPrivateProfileString("settings", "stats", "true", ret, 2083, _configFilepath) > 0)
            {
                bool stats;
                if (bool.TryParse(ret.ToString(), out stats))
                    Stats = stats;
            }
            else
            {
                Stats = true;
            }

            this.Guid = NativeMethods.GetPrivateProfileString("settings", "guid", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : System.Guid.NewGuid().ToString();

            // ReSharper disable once InvertIf
            if (NativeMethods.GetPrivateProfileString("settings", "debug", "", ret, 2083, _configFilepath) > 0)
            {
                bool debug;
                if (bool.TryParse(ret.ToString(), out debug))
                    Debug = debug;
            }
        }

        static string GetConfigFilePath()
        {
            //var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // get path of plugin configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            string iniFilePath = sbIniFilePath.ToString();

            return Path.Combine(iniFilePath, "CodeStats_custom_extension_mapping.ini");
        }
    }
}