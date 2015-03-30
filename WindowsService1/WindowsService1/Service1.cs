using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Data.SqlClient;


namespace PHEAAScheduleService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FileStream fs = new FileStream(@"c:\temp\pheaa.txt", FileMode.CreateNew, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs); 

            m_streamWriter.WriteLine(" mcWindowsService: Service Started \n"); 
            m_streamWriter.Flush();
            m_streamWriter.Close(); 

        }

        protected override void OnStop()
        {
        }
        protected override void OnContinue()
        {
        }

        protected override void OnPause()
        {
        }
    }
}
