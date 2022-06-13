using LightMixer;
using System.ServiceProcess;

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
