using System;
using System.Windows;

namespace LightMixer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            if (System.Diagnostics.Process.GetProcessesByName("LightMixer").Length > 1)
            {
                MessageBox.Show("Already Running");
                this.Shutdown();
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            new BootStrap(new UiDispatcher(Dispatcher));
        }


        private void App_Exit(object sender, ExitEventArgs e)
        {

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exceptionTest = e.ExceptionObject as Exception;
            if (exceptionTest != null)
            {
                MessageBox.Show(exceptionTest.ToString());
            }
            else
            {
                MessageBox.Show("Unknow exception");
            }
        }


    }
}
