using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DmxLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DmxService" in both code and config file together.
    public class DmxService : IDmxService
    {
        public void SetDmxChannel(int channel, byte value)
        {
            DmxController.dmx.SetDmxValue(channel, value);
        }

        public void SetBreak(long value)
        {
            //DmxController.dmx.DMXSetting.BreakLenght = value;
        }

        public void SetMBB(long value)
        {
            //DmxController.dmx.DMXSetting.MBB = value;
        }

        public void SetMab(long value)
        {
            //DmxController.dmx.DMXSetting.MAB = value;
        }


        public void UpdateAllDmxValue(byte[] value)
        {
            DmxController.dmx.Buffer = value;
            
        }

        
    }
}
