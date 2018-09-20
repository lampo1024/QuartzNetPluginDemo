using Quartz;
using System;

namespace TaskDemo.Shared
{
    public interface IQuartzPlugin:IJob
    {
        string PluginName { get; }
        string JobKey { get; }
        string JobGroupName { get; }
        string JobTriggerName { get; }

        void Initialize(IScheduler scheduler);
        void Starting();
        void Finished();
        void Start();
        void Stop();
    }
}
