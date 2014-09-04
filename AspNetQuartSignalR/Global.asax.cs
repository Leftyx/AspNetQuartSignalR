using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace AspNetQuartSignalR
{
    using Quartz;
    using Quartz.Impl;
    using AspNetQuartSignalR.Scheduler;

    public class Global : HttpApplication
    {
        public static ISchedulerFactory SchedulerFactory;
        public static IScheduler Scheduler;
        public static QuartzSchedulerService SchedulerService;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            // RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SchedulerFactory = new StdSchedulerFactory();
            Scheduler = SchedulerFactory.GetScheduler();

            SchedulerService = new QuartzSchedulerService(SchedulerFactory, Scheduler);
            SchedulerService.ScheduleJob();
        }
    }
}