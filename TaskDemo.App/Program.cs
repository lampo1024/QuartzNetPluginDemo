using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TaskDemo.Shared;

namespace TaskDemo.App
{
    class Program
    {
        private static IScheduler _scheduler;
        static void Main(string[] args)
        {
            Init();
            Console.ReadKey();
        }


        static void Init()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            LoadPlugins();
        }

        static void LoadPlugins()
        {
            var assDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            var plugins = PluginLoader.GetInterfaceImplementor<IQuartzPlugin>(assDir);
            foreach (var plugin in plugins)
            {
                plugin.Initialize(_scheduler);
                plugin.Start();
            }
        }

    }
}
