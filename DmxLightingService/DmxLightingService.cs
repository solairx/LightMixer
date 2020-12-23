using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using DmxLib;
using LightMixer;

namespace DmxLightingService
{
    public partial class DmxLightingService : ServiceBase
    {
        private BootStrap bootstrap;

        public DmxLightingService()
        {
            InitializeComponent();
            this.ServiceName = "Dmx Lighting Servive";
            
        }

        protected override void OnStart(string[] args)
        {
            bootstrap = new BootStrap(new ServiceDispatcher());
            // var bootstrap = new BootStrap();
        }

        protected override void OnStop()
        {
        }


    }
}
