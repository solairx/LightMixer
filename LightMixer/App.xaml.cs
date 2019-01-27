using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using LightMixer.Model.Service;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using Phidgets;
using Phidgets.Events;
using LightMixer.View;

namespace LightMixer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private InterfaceKit kit;

        public IUnityContainer UnityContainer
        {
            get;
            set;
        }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                kit = new InterfaceKit();

                kit.open("desktop-pjdgjgm", 5001);

                kit.waitForAttachment(250);
                kit.InputChange += new InputChangeEventHandler(kit_InputChange);
            }
            catch (Exception vepx)
            {
             //   MessageBox.Show("Unable to connecto to the PhidGet, that might be expected");
            }
            UnityContainer = new UnityContainer();
            SharedEffectModel mn = new SharedEffectModel();
            UnityContainer.RegisterInstance<SharedEffectModel>(mn);
            UnityContainer.RegisterInstance<LightService.DmxServiceClient>(new LightService.DmxServiceClient());
            DmxModel model = new DmxModel();
            UnityContainer.RegisterInstance<DmxChaser>(new DmxChaser(model));
            
            UnityContainer.RegisterInstance<DmxModel>(model);
            try
            {
                ServiceHost wcfHost = new ServiceHost(typeof(RemoteLightService));
                wcfHost.Open();
            } 
            catch (Exception vexp)
            {
                MessageBox.Show("Can't open wcf service for Listen, " + vexp.Message);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exceptionTest = e.ExceptionObject as Exception;
            if (exceptionTest !=null)
            {
                MessageBox.Show(exceptionTest.ToString());
            }
            else
            {
                MessageBox.Show("Unknow exception");
            }

            
        }

        private void kit_InputChange(object sender, InputChangeEventArgs e)
        {
            if (e.Index == 5)
            {
                var chaser = ((App)App.Current).UnityContainer.Resolve<DmxChaser>();
                var shared = ((App)App.Current).UnityContainer.Resolve<SharedEffectModel>();
                var beatDetector = ((App)App.Current).UnityContainer.Resolve<BeatDetector.BeatDetector>();
                shared.Dispatcher.Invoke(new Action(() =>
                {

                    chaser.CurrentLedEffect = chaser.LedEffectCollection.FirstOrDefault(o => o.Name.Contains("On"));

                    shared.AutoChangeColorOnBeat = false;
                    if (e.Value)
                    {
                        shared.MaxLightIntesity = 64;
                    }
                    else
                    {
                        shared.MaxLightIntesity =0;
                    }

                    shared.Green = 255;
                    shared.Blue = 255;
                    shared.Red = 255;

                }));
            }
        }
    }
}
