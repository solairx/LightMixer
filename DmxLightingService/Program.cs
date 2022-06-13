using System.ServiceProcess;

namespace DmxLightingService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DmxLightingService()
            };
            ServiceBase.Run(ServicesToRun);

        }
    }
}
