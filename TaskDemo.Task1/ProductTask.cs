using System;
using System.Threading.Tasks;
using Quartz;
using TaskDemo.Shared;

namespace TaskDemo.Task1
{
    public class ProductTask : IQuartzPlugin
    {
        private IScheduler _scheduler;
        public string PluginName => "Plugin two";

        public string JobKey => "JobKey_2";

        public string JobGroupName => "JobGroupName_2";

        public string JobTriggerName => "JobTriggerName_2";

        public Task Execute(IJobExecutionContext context)
        {
            //    JobKey key = context.JobDetail.Key;

            //    JobDataMap dataMap = context.JobDetail.JobDataMap;

            //    string MethodName = dataMap.GetString("MethodName");
            Console.WriteLine($"Task two : {DateTime.Now.ToString()}");
            return Task.FromResult(0);

        }

        public void Finished()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IScheduler sched)
        {
            _scheduler = sched;
            Starting();
        }

        public void Start()
        {
            _scheduler.Start();
        }


        public void Starting()
        {
            Console.WriteLine("starting ProductTask of task 1 ...");
            JobDataMap jobData = new JobDataMap();
            //jobData["ConnectionString"] = ConnectionString;

            IJobDetail job = JobBuilder.Create(this.GetType())
                .WithDescription("Job to rescan jobs from SQL db")
                .WithIdentity(new JobKey(PluginName, JobGroupName))
                .UsingJobData(jobData)
                .Build();

            TriggerKey triggerKey = new TriggerKey(JobTriggerName, JobGroupName);

            ITrigger trigger = TriggerBuilder.Create()
                .WithCronSchedule("*/5 * * * * ?")
                //.WithCalendarIntervalSchedule((d) => { d.WithIntervalInSeconds(2); })
                .StartNow()
                .WithDescription("trigger for sql job loader")
                .WithIdentity(triggerKey)
                .WithPriority(1)
                .Build();

            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.PauseJob(new JobKey(JobKey));
        }
    }
}
