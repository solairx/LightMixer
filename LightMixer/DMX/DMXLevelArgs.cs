using System;

namespace LightMixer
{
    public class DMXLevelArgs : EventArgs
    {
        public bool valid;
        public byte[] levels;
        public DMXLevelArgs(bool isValid, byte[] lvls)
        {
            valid = isValid;
            levels = lvls;
        }
    }
}
