//using Quartz;
//using Quartz.Impl;

namespace Land.Schedule
{
    // WORKING SHEDULER (please not delete just in case we will use it simetimes)
    //public class DeleteAdTask : IJob
    //{
    //    public Task Execute(IJobExecutionContext context)
    //    {
    //        var task = Task.Run(() => logfile(DateTime.Now)); ;
    //        return task;
    //    }
    //    public void logfile(DateTime time)
    //    {
    //        string path = "C:\\log\\sample.txt";
    //        using (StreamWriter writer = new StreamWriter(path, true))
    //        {
    //            writer.WriteLine(time);
    //            writer.Close();
    //        }
    //    }
    //}

    //public class SchedulerTask
    //{
    //    private static readonly string ScheduleCronExpression = "* * * ? * *";
    //    public static async System.Threading.Tasks.Task StartAsync()
    //    {
    //        try
    //        {
    //            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
    //            if (!scheduler.IsStarted)
    //            {
    //                await scheduler.Start();
    //            }
    //            var job1 = JobBuilder.Create<DeleteAdTask>().WithIdentity("ExecuteTaskServiceCallJob1", "group1").Build();
    //            var trigger1 = TriggerBuilder.Create().WithIdentity("ExecuteTaskServiceCallTrigger1", "group1").WithCronSchedule(ScheduleCronExpression).Build();
    //            await scheduler.ScheduleJob(job1, trigger1);
    //        }
    //        catch (Exception ex) { }
    //    }
    //}
}
