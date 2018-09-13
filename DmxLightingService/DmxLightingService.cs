using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DmxLib;

namespace DmxLightingService
{
    public partial class DmxLightingService : ServiceBase
    {
        private DmxController  controller;
        public DmxLightingService()
        {
            InitializeComponent();
            this.ServiceName = "Dmx Lighting Servive";
            
        }

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            controller = new DmxController();
            controller.Start();
        }

        protected override void OnStop()
        {
            controller.Stop();
        }


    }
}
