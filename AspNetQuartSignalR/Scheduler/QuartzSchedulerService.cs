using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetQuartSignalR.Scheduler
{
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.Triggers;
    using System.Diagnostics;

    public class QuartzSchedulerService
    {
        private readonly ISchedulerFactory SchedulerFactory;
        private readonly IScheduler Scheduler;

        public QuartzSchedulerService(ISchedulerFactory schedulerFactory, IScheduler scheduler)
        {
            this.SchedulerFactory = schedulerFactory;
            this.Scheduler = scheduler;
        }

        public void ScheduleJob()
        {
            IJobDetail helloJob = null;
            ITrigger helloTrigger = null;

            helloJob = this.Scheduler.GetJobDetail(new JobKey("HelloJob", "MyGroup"));

            if (helloJob == null)
            {
                helloJob = JobBuilder.Create<Jobs.HelloJob>()
                                           .WithIdentity("HelloJob", "MyGroup")
                                           .RequestRecovery(true)
                                           .StoreDurably(true)
                                           .Build();

                helloTrigger = TriggerBuilder.Create()
                                                 .WithIdentity("HelloTrigger", "MyGroup")
                                                 .StartNow()
                                                 .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(15))
                                                 .Build();

                this.Scheduler.ScheduleJob(helloJob, helloTrigger);
            }

            this.Scheduler.Start();

            var nextFire = GetNextFireTimeForJob("HelloJob", "MyGroup");
        }

        public void Stop()
        {
            this.Scheduler.Shutdown(false);
        }


        public DateTime GetNextFireTimeForJob(string jobName, string groupName)
        {
            JobKey jobKey = new JobKey(jobName, groupName);
            DateTime nextFireTime = DateTime.MinValue;

            bool isJobExisting = Scheduler.CheckExists(jobKey);
            if (isJobExisting)
            {
                var detail = this.Scheduler.GetJobDetail(jobKey);
                var triggers = this.Scheduler.GetTriggersOfJob(jobKey);

                if (triggers.Count > 0)
                {
                    var nextFireTimeUtc = triggers[0].GetNextFireTimeUtc();
                    nextFireTime = TimeZone.CurrentTimeZone.ToLocalTime(nextFireTimeUtc.Value.DateTime);
                }
            }

            return (nextFireTime);
        }

    }
}