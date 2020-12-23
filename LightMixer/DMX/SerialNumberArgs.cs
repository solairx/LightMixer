using System;

namespace LightMixer
{
    public class SerialNumberArgs : EventArgs
    {
        public UInt32 SerialNumber;
        public SerialNumberArgs(UInt32 serial)
        {
            SerialNumber = serial;
        }
    }
}
