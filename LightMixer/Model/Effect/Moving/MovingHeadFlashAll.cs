﻿using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class MovingHeadFlashAll : EffectBase
    {

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {

            foreach (FixtureBase fixture in Owner.FixtureGroups.SelectMany(o => o.FixtureInGroup))
            {
                if (fixture is MovingHeadFixture)
                {
                    ((MovingHeadFixture)fixture).ColorMode = MovingHeadFixture.ColorMacro.Auto;
                    ((MovingHeadFixture)fixture).GoboPaturn = _sharedEffectModel.CurrentMovingHeadGobo;
                    ((MovingHeadFixture)fixture).Pan = 32000;
                    if (DateTime.Now.Subtract(_sharedEffectModel.IsCurrentMovingHeadProgramDirty).TotalMilliseconds < 250)
                    {
                        ((MovingHeadFixture)fixture).ProgramMode = _sharedEffectModel.CurrentMovingHeadProgram;
                    }
                    ((MovingHeadFixture)fixture).Speed = this.GetMaxedByte(this._sharedEffectModel.MaxSpeed * this.bpm);
                    if (isBeat || isSimulatedBeat)
                    {
                        ((MovingHeadFixture)fixture).RedValue = this.SetValueMovingHead(this._sharedEffectModel.Red);
                        ((MovingHeadFixture)fixture).GreenValue = this.SetValueMovingHead(this._sharedEffectModel.Green);
                        ((MovingHeadFixture)fixture).BlueValue = this.SetValueMovingHead(this._sharedEffectModel.Blue);
                    }
                    else
                    {

                        ((MovingHeadFixture)fixture).RedValue = 0;
                        ((MovingHeadFixture)fixture).GreenValue = 0;
                        ((MovingHeadFixture)fixture).BlueValue = 0;
                    }
                }
            }

            if (isBeat || isSimulatedBeat)
            {
                isBeat = false;
                isSimulatedBeat = false;
            }




            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "MovingFlashAll";
            }

        }
    }
}
