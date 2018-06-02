using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    public interface IJob
    {
        RegisterdJob Register { get; set; }
        RegisterdJob[] Dependencys { get; }

        // TODO : implement this
        RegisterdJob[] ScheduleBefore { get; }

        void Execute(ScheduledJob[] dependencys);
    }

    public class RegisterdJob
    {
        public int Id;

        public IJob Job;
    }

    public class ScheduledJob
    {
        public IJob Job { get; set; }
        //public RegisterdJob Register { get; set; }

        public int DependencyMissing { get; set; }
        public bool IsReady { get { return DependencyMissing <= 0; } }
        public bool IsCompleted { get; protected set; }

        public void WaitForCompletion()
        {
            // TODO 
        }

        public void MarkDone()
        {
            IsCompleted = true;
        }
    }

    public interface IJobSource
    {
        void SheduleJobs(JobSystem jobSystem, ScheduleScope scope);
    }

    public enum ScheduleScope
    {
        Frame,
        Background,
    }

    public class JobSystem : System
    {
        public RegisterdJob Register(IJob job)
        {
            RegisterdJob rj = new RegisterdJob
            {
                Job = job,
            };
            job.Register = rj;
            return rj;
        }

        public void BeginFrame()
        {
            foreach(IJobSource source in context.ListSystems<IJobSource>())
            {
                source.SheduleJobs(this, ScheduleScope.Frame);
            }
        }
        
        Queue<ScheduledJob> readyBuffer = new Queue<ScheduledJob>();
        Dictionary<RegisterdJob, List<ScheduledJob>> dependencyGraph = new Dictionary<RegisterdJob, List<ScheduledJob>>();

        // TODO : allow frame independend Jobs to be run in background
        public ScheduledJob Schedule(IJob job, ScheduleScope scope)
        {
            ScheduledJob sj = new ScheduledJob
            {
                Job = job,
                //Register = null, // TODO
                DependencyMissing = job.Dependencys.Length,
            };

            foreach(RegisterdJob dependency in job.Dependencys)
            {
                List<ScheduledJob> jobs;
                if(!dependencyGraph.TryGetValue(dependency, out jobs))
                {
                    jobs = new List<ScheduledJob>();
                    dependencyGraph.Add(dependency, jobs);
                }
                jobs.Add(sj);
            }

            if(sj.IsReady)
            {
                readyBuffer.Enqueue(sj);
            }

            return sj;
        }

        public RegisterdJob GetScheduledJob<T>()
        {
            // TODO
            return null;
        }

        void OnJopComplite(ScheduledJob sj)
        {
            sj.MarkDone();

            // TODO : we can make this loop use a fixed buffer by using the Iterator and state management
            List<ScheduledJob> jobs;
            if(dependencyGraph.TryGetValue(sj.Job.Register, out jobs))
            {
                List<ScheduledJob> jobsReady = new List<ScheduledJob>();

                foreach(ScheduledJob depJob in jobs)
                {
                    depJob.DependencyMissing--;
                    if(depJob.IsReady)
                    {
                        jobsReady.Add(depJob);
                    }
                }

                foreach(ScheduledJob ready in jobsReady)
                {
                    jobs.Remove(ready);
                    readyBuffer.Enqueue(ready);
                }
            }
        }

        public void CompliteFrame()
        {
            while(readyBuffer.Count > 0)
            {
                ScheduledJob sj = readyBuffer.Dequeue();
                sj.Job.Execute(null);
                OnJopComplite(sj);
            }
        }
    }
}
