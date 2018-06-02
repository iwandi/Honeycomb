using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb.Windows
{
    public class InputSystem : System, IJobSource
    {
        [Import]
        JobSystem JobSystem;

        public InputCollectJob CollectJob { get; protected set; }
        public RegisterdJob CollectRegJob { get; protected set; }

        public override void Init()
        {
            CollectJob = new InputCollectJob();
            CollectRegJob = JobSystem.Register(CollectJob);
        }

        public void SheduleJobs(JobSystem jobSystem, ScheduleScope scope)
        {
            jobSystem.Schedule(CollectJob, scope);
        }

        public class InputCollectJob : IJob
        {
            public RegisterdJob Register { get; set; }
            public RegisterdJob[] Dependencys => throw new NotImplementedException();

            public void Execute(ScheduledJob[] dependencys)
            {

            }
        }
    }
}
