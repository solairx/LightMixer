using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using VisualControler;
//using System.Windows.Forms;


namespace SolairXDj
{
    public partial class LedDemo // : Form
    {
        private Phidgets.InterfaceKit iface;
        private dj djSetting = new dj();
        
        public bool ready = false;
        public LedDemo()
        {
            //InitializeComponent();
            try
            {
           
                iface = new Phidgets.InterfaceKit();
  
                iface.open(djSetting.PhidgetLEDSn);

                iface.waitForAttachment(1000);

            }
            catch (Exception d)
            {
               // this.toolStripStatusLabelInterfaceFound.Text = "Interface Not Found";
            }
            
         
        }

        public void ChangeLed(int index, bool value)
        {
            try
            {
                if (iface.Attached)
                {
                    iface.outputs[index] = value;
                    if (!ready)
                    {
                        ready = true;
                      //  this.toolStripStatusLabelInterfaceFound.Text = "Interface Attached";
                    }
                }
                else
                {
                 //   this.toolStripStatusLabelInterfaceFound.Text = "Interface Not Found";
                }
            }
            catch (Exception e)
            {
                ready = false;
             //   this.toolStripStatusLabelInterfaceFound.Text = e.ToString();
            }
            //if (index == 0) this.progressBar1.Value = !value ? 0 : 1;
            //if (index == 1) this.progressBar2.Value = !value ? 0 : 1;
            //if (index == 2) this.progressBar3.Value = !value ? 0 : 1;
            //if (index == 3) this.progressBar4.Value = !value ? 0 : 1;
            //if (index == 4) this.progressBar5.Value = !value ? 0 : 1;
            //if (index == 5) this.progressBar6.Value = !value ? 0 : 1;
            //if (index == 6) this.progressBar7.Value = !value ? 0 : 1;
            //if (index == 7) this.progressBar8.Value = !value ? 0 : 1;
            //if (index == 8) this.progressBar9.Value = !value ? 0 : 1;
            //if (index == 9) this.progressBar10.Value =!value ? 0 : 1;
            //if (index == 10) this.progressBar11.Value = !value ? 0 : 1;
            //if (index == 11) this.progressBar12.Value = !value ? 0 : 1;
            //if (index == 12) this.progressBar13.Value = !value ? 0 : 1;
            //if (index == 13) this.progressBar14.Value = !value ? 0 : 1;
            //if (index == 14) this.progressBar15.Value = !value ? 0 : 1;
            //if (index == 15) this.progressBar16.Value = !value ? 0 : 1;
        }
        public bool GetLed(int index)
        {
            if ((iface != null) & iface.Attached)
                return iface.outputs[index];
            else return false;
        }

        //private void LedDemo_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide();
        //}
    }
}
