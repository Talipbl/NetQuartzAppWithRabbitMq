using Quartz;
using Quartz.Impl;
using SaleManagement.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleManagement
{
    public static class Trigger
    {
        private static async Task<IScheduler> Start()
        {
            IScheduler scheduler = await new StdSchedulerFactory().GetScheduler();
            if (!scheduler.IsStarted)
            {
                await scheduler.Start();
            }
            return scheduler;
        }
        public static async void TriggerTask()
        {
            IScheduler scheduler = await Start();

            IJobDetail readJob = JobBuilder.Create<ReadQueue>().WithIdentity("readQueue").Build();

            ISimpleTrigger readTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("readQueue")
                .StartAt(DateTime.Now.AddSeconds(65))
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();

            await scheduler.ScheduleJob(readJob, readTrigger);
        }
    }
}
