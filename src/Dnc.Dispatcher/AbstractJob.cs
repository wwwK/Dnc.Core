﻿using Dnc.Output;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dnc.Dispatcher
{
    /// <summary>
    /// Extention abstract class  from <see cref="IJob"/>
    /// </summary>
    public abstract class AbstractJob: IJob
    {
        protected ILogger<AbstractJob> mLogger => Fx.Resolve<ILogger<AbstractJob>>();
        protected IConsoleOutputHelper mOutput => Fx.Resolve<IConsoleOutputHelper>();
        public async Task Execute(IJobExecutionContext context)
        {
            var name = context.JobDetail.JobDataMap.GetString("name");

            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            var sw = new Stopwatch();
            mOutput.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}开始执行{name}");
            sw.Start();
            await ExecuteJobAsync(context);
            sw.Stop();
            mOutput.Info($"执行结束,总耗时{sw.Elapsed.Hours}:{sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}:{sw.Elapsed.Milliseconds}");
            Console.ForegroundColor = oldColor;
        }

        public abstract Task ExecuteJobAsync(IJobExecutionContext context);
    }
}
