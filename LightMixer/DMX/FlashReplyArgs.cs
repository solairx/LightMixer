using System;

namespace LightMixer
{
    public class FlashReplyArgs : EventArgs
    {
        public bool success;

        public FlashReplyArgs(bool s)
        {
            success = s;
        }
    }
}