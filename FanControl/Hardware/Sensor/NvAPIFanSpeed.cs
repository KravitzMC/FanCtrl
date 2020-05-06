﻿using Gigabyte.Engine.EnvironmentControl.CoolingDevice.Fan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanControl
{
    public class NvAPIFanSpeed : BaseSensor
    {
        public delegate int OnGetNvAPIFanSpeedHandler(int index, int coolerID);

        public event OnGetNvAPIFanSpeedHandler onGetNvAPIFanSpeedHandler;

        private int mIndex = 0;
        private int mCooerID = 0;
        
        public NvAPIFanSpeed(string name, int index, int coolerID) : base(SENSOR_TYPE.FAN)
        {
            Name = name;
            mIndex = index;
            mCooerID = coolerID;
        }

        public override string getString()
        {
            var valueString = string.Format("{0:D4}", Value);
            return valueString + " RPM";
        }

        public override void update()
        {
            Value = onGetNvAPIFanSpeedHandler(mIndex, mCooerID);
        }
        
    }
}
