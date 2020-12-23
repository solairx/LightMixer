using System;

namespace LightMixer
{
    public class DMXMessage : EventArgs
    {
        public DMXProMsgLabel type;
        public byte[] message;

        public DMXMessage(DMXProMsgLabel t, byte[] m)
        {
            message = m;
            type = t;
        }
    }
}
