using System;

namespace LightMixer
{
    public class WidgetParameterArgs : EventArgs
    {
        public UInt16 Firmware;
        public double DMXOutBreakTime;
        public double DMXOutMarkTime;
        public int DMXOutRate;
        public byte[] UserConfigData;

        public WidgetParameterArgs(ushort dmxFirmware, double dmxOutBreakTime, double dmxOutMarkTime, int dmxOutRate, byte[] ConfigData)
        {
            Firmware = dmxFirmware;
            DMXOutBreakTime = dmxOutBreakTime;
            DMXOutMarkTime = dmxOutMarkTime;
            DMXOutRate = dmxOutRate;
            UserConfigData = ConfigData;
        }
    }
}
