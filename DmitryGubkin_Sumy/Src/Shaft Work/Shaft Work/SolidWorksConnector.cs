using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;

//Special class for creatin connection between program and SolidWorks
namespace Shaft_Work
{
     class SolidWorksConnector
    {
        private SldWorks SwApp = null;
        private ModelDoc2 SwModel = null;

         public SolidWorksConnector()
        {
            
        }

        ~SolidWorksConnector()
        {
            GC.Collect();
        }

        public SldWorks MyApp
        {
            get { return SwApp; }
          
        }

        public ModelDoc2 MySwModel
        {
            get { return SwModel; }
           
        }


        public void SldConnect()
        {

            try
            {
                System.Diagnostics.Process[] sw = System.Diagnostics.Process.GetProcessesByName("SLDWORKS");
                object processSw = null;
                if (sw.Length > 1) // kill if more then one copy of sw app is runned
                {
                    foreach (System.Diagnostics.Process sw_close in sw)
                    {
                        sw_close.CloseMainWindow();
                        sw_close.Kill();

                    }
                }
                else
                {
                    processSw = System.Activator.CreateInstance(System.Type.GetTypeFromProgID("SldWorks.Application"));
                    // create or get access to the existace proccess
                    if (processSw == null)
                    {
                        MessageBox.Show(@"Looks like You haven't any installed copy of Solidworks yet");
                    }
                }

                SwApp = (SldWorks) processSw;
                if (SwApp == null) return;
                SwApp.Visible = true;
                SwApp.UserControl = true;
                ////
                SwApp.INewPart();
                SwModel = (ModelDoc2) SwApp.NewDocument(Application.StartupPath + @"tamplate.prtdot", 100, 0, 0);
                SwModel = SwApp.IActiveDoc2;
            }

            catch (Exception)
            {
                MessageBox.Show(@"Can't connect to SolidWorks");
            }

        }

         public void SldKill()
         {
             try
             {
                System.Diagnostics.Process[] sw = System.Diagnostics.Process.GetProcessesByName("SLDWORKS");
                object processSw = null;
                if (sw.Length > 0) // kill 
                {
                    foreach (System.Diagnostics.Process sw_close in sw)
                    {
                        sw_close.CloseMainWindow();
                        sw_close.Kill();

                    }
                }

                 SwModel = null;
                 SwApp = null;
                GC.Collect();
             }
             catch (Exception)
             {
                 
                 throw;
             }
         }



    }
}
