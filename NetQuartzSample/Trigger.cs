using NetQuartzSample.Tasks;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetQuartzSample
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
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever()).Build();

            IJobDetail writeJob = JobBuilder.Create<WriteQueue>().WithIdentity("writeQueue").Build();

            ISimpleTrigger writeTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("writeQueue")
                .StartAt(DateTime.Now.AddMinutes(1))
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();

            await scheduler.ScheduleJob(readJob, readTrigger);
            await scheduler.ScheduleJob(writeJob, writeTrigger);
        }
    }
}
