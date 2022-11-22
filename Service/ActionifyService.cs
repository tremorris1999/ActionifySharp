using System;
using System.Collections.Generic;
using ActionifySharp.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ActionifySharp.Service;

public static class ActionifyService
{
  public static void AddActionify(this IServiceCollection services )
  {
    services.AddSingleton<ScheduleWorker>();
    services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<ScheduleWorker>());
  }

  public static void Actionify(this IHost host, Func<ScheduleWorker, IEnumerable<WorkerAction>> tasks)
  {
    ScheduleWorker worker = host.Services.GetService<ScheduleWorker>() ?? throw new ApplicationException("Actionify service not added to services.");
    worker.AddActions(tasks.Invoke(worker));
  }

  public static void Actionify(this IHost host, Func<ScheduleWorker, WorkerAction> task)
  {
    ScheduleWorker worker = host.Services.GetService<ScheduleWorker>() ?? throw new ApplicationException("Actionify service not added to services.");
    worker.AddAction(task.Invoke(worker));
  }
}