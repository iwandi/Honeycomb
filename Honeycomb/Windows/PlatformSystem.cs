using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Honeycomb;

namespace Honeycomb.Windows
{
    public class PlatformSystem : System, IPlatformSystem, IJobSource
    {
        [Import]
        JobSystem jobSystem;

        Form window;

        public bool Run
        {
            get
            {
                if(window != null)
                {
                    return !window.IsDisposed;
                }
                return true;
            }
        }

        public PlatformUpdateJob PlatformUpdate { get; protected set; }
        public RegisterdJob PlatformUpdateRegJob { get; protected set; }

        public override void Init()
        {
            window = new Form();
            window.Show();
            window.ClientSize = new Size(1280, 720);

            PlatformUpdate = new PlatformUpdateJob();
            PlatformUpdateRegJob = jobSystem.Register(PlatformUpdate);
        }

        public void SheduleJobs(JobSystem jobSystem, ScheduleScope scope)
        {
            jobSystem.Schedule(PlatformUpdate, scope);
        }

        public void Shutdown()
        {
            window.Close();
        }

        public class PlatformUpdateJob : IJob
        {
            public RegisterdJob Register { get; set; }
            public RegisterdJob[] Dependencys { get { return new RegisterdJob[0]; } }

            public void Execute(ScheduledJob[] dependencys)
            {
                Application.DoEvents();
            }
        }
    }
}
