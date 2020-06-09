﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.ServiceModel;
using System.Windows;
using LightMixer.View;
using System.Web.Http.SelfHost;
using System.Web.Http;

namespace LightMixer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            new BootStrap();
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
