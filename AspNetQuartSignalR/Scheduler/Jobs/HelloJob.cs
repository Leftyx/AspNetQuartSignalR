using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetQuartSignalR.Scheduler.Jobs
{
    using Microsoft.AspNet.SignalR;

    public class HelloJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var quartzHub = GlobalHost.ConnectionManager.GetHubContext<AspNetQuartSignalR.Hubs.QuartzHub>();

            string message = string.Format("Hello Job execution finished: {0}", DateTime.Now.ToString());

            System.Threading.Thread.Sleep(5000);

            quartzHub.Clients.All.quartzJobExecuted(message);

        }
    }
}