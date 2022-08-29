using Quartz;
using Quartz.Impl;


Trigger.TriggerTask();
Console.ReadLine();


class Mission : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Job Çalıştı.");
        return Task.CompletedTask;
    }
}

static class Trigger
{
    private static async Task<IScheduler> Start()
    {
        ISchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();
        if (!scheduler.IsStarted)
        {
            await scheduler.Start();
        }
        return scheduler;

    }

    public static async void TriggerTask()
    {
        IScheduler scheduler = await Start();
        IJobDetail job = JobBuilder.Create<Mission>().WithIdentity("mission").Build();
        ISimpleTrigger triggerJob = (ISimpleTrigger)TriggerBuilder.Create()
            .WithIdentity("mission")
            .StartAt(DateTime.Now)
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();

        await scheduler.ScheduleJob(job, triggerJob);
    }
}
