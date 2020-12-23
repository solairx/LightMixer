﻿using System;
using System.Linq;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using Phidgets;
using Phidgets.Events;
using System.Diagnostics;

namespace LightMixer
{
    public class PhidGetService
    {
        private InterfaceKit kit;

        public PhidGetService()
        {
            try
            {
                kit = new InterfaceKit();

                kit.open();

                kit.waitForAttachment(500);
                kit.InputChange += new InputChangeEventHandler(kit_InputChange);
            }
            catch (Exception vepx)
            {
                Debug.WriteLine(vepx.ToString());
            }
        }

        private void kit_InputChange(object sender, InputChangeEventArgs e)
        {
            if (e.Index == 5)
            {
                var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
                var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
                BootStrap.Dispatcher.Invoke(new Action(() =>
                {
                    shared.AutoChangeColorOnBeat = false;

                    if (e.Value)
                    {
                        chaser.CurrentLedEffect = chaser.LedEffectCollection.FirstOrDefault(o => o.Name.Contains("On"));
                    }
                    else
                    {
                        chaser.CurrentLedEffect = chaser.LedEffectCollection.FirstOrDefault(o => o.Name.Contains("Off"));
                    }

                    shared.Green = 255;
                    shared.Blue = 255;
                    shared.Red = 255;

                }));
            }
        }
    }
}