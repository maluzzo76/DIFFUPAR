using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ComplementosServices
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(20000);

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.Write.WriteError("Inicio Servicio");
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                Log.Write.WriteError("Inicio Elapsed Servicio");
                Process.IA.ProcesarComplementos();
            }
            catch (Exception ex)
            {             
                Log.Write.WriteException(ex);
            }
            finally
            { timer.Start(); }
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
