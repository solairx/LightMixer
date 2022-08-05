using System;

namespace BeatDetector
{
    public class ZplanePoi : VDJPoi
    {
        public ZplanePoi(int energyPos, int energyMax, int g, int b, string pos, VDJScan vDJScan)
            : base("E=" + energyPos + "G=" + g + "B=" + b, pos, "Zplane", vDJScan)
        {
            EnergyPos = energyPos;
            EnergyMax = energyMax;
            G = g;
            B = b;
        }

        public int EnergyPos { get; }
        public int EnergyMax { get; }
        public int G { get; }
        public int B { get; }

        public override bool IsBreak => this.EnergyPos >= 2 && this.EnergyPos <= GetMaxEnergyForVerse();
        private int GetMaxEnergyForVerse()
        {
            return EnergyMax - 2;
        }

        public override bool IsEndBreak => this.EnergyPos < 2 || this.EnergyPos > GetMaxEnergyForVerse();
    }
}