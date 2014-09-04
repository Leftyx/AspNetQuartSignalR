using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace AspNetQuartSignalR.Hubs
{
    using Quartz.Impl.Matchers;
    using Quartz;

    public class QuartzHub : Hub
    {
        public QuartzHub()
        {
        }

        public void CheckQuartzStatus()
        {
            string message = string.Empty;

            var allTriggerKeys = Global.Scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            foreach (var triggerKey in allTriggerKeys)
            {
                ITrigger trigger = Global.Scheduler.GetTrigger(triggerKey);
                message += string.Format("{0} = {1}", trigger.Key, Global.Scheduler.GetTriggerState(trigger.Key)) + Environment.NewLine;
            }

            Clients.All.onCheckQuartzStatus(message);
        }

        public void StopQuartzExecution()
        {
            Global.Scheduler.Standby();
            Clients.All.onCheckQuartzStatus("Quartz is in stand-by.");
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

    }
}