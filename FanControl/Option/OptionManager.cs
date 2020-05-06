﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FanControl
{
    public enum LibraryType
    {
        LibreHardwareMonitor = 0,
        OpenHardwareMonitor,
    };

    public class OptionManager
    {
        private const string cOptionFileName = "Option.json";

        private static OptionManager sManager = new OptionManager();
        public static OptionManager getInstance() { return sManager; }

        private StartupControl mStartupControl = new StartupControl();

        private OptionManager()
        {
            Interval = 1000;
            LibraryType = LibraryType.LibreHardwareMonitor;
            IsNvAPIWrapper = false;
            IsMinimized = false;
        }
        
        public int Interval { get; set; }

        public LibraryType LibraryType { get; set; }

        public bool IsNvAPIWrapper { get; set; }

        public bool IsMinimized { get; set; }

        public bool IsStartUp
        {
            get
            {
                return mStartupControl.Startup;
            }
            set
            {

                mStartupControl.Startup = value;
            }
        }

        public bool read()
        {
            try 
            {
                var jsonString = File.ReadAllText(cOptionFileName);                
                var rootObject = JObject.Parse(jsonString);

                Interval = (rootObject.ContainsKey("interval") == true) ? rootObject.Value<int>("interval") : 1000;

                if (rootObject.ContainsKey("library") == false)
                    LibraryType = LibraryType.LibreHardwareMonitor;
                else
                    LibraryType = (rootObject.Value<int>("library") == 0) ? LibraryType.LibreHardwareMonitor : LibraryType.OpenHardwareMonitor;

                IsNvAPIWrapper = (rootObject.ContainsKey("nvapi") == true) ? rootObject.Value<bool>("nvapi") : false;
                IsMinimized = (rootObject.ContainsKey("minimized") == true) ? rootObject.Value<bool>("minimized") : false;
                IsStartUp = (rootObject.ContainsKey("startup") == true) ? rootObject.Value<bool>("startup") : false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void write()
        {
            try
            {
                var rootObject = new JObject();
                rootObject["interval"] = Interval;
                rootObject["library"] = (LibraryType == LibraryType.LibreHardwareMonitor) ? 0 : 1;
                rootObject["nvapi"] = IsNvAPIWrapper;
                rootObject["minimized"] = IsMinimized;
                rootObject["startup"] = IsStartUp;
                File.WriteAllText(cOptionFileName, rootObject.ToString());
            }
            catch {}
        }

    }
}
