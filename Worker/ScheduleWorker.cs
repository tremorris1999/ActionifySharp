using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ActionifySharp.Worker;

public class ScheduleWorker : BackgroundService
{
  private readonly ILogger<ScheduleWorker> _logger;
  public IEnumerable<WorkerAction> Actions { get; set; } = new List<WorkerAction>();

  public ScheduleWorker(ILogger<ScheduleWorker> logger)
  {
    _logger = logger;
  }

  public void AddActions(IEnumerable<WorkerAction> actions) => ((List<WorkerAction>)Actions).AddRange(actions);
  public void AddAction(WorkerAction action) => ((List<WorkerAction>)Actions).Add(action);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      IEnumerable<WorkerAction> actions = Actions.Where(action => action.NextExecution <= DateTime.Now);
      foreach (WorkerAction action in actions)
      {
        _logger.LogInformation("Running action {name} at {time}", action.Name, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        try
        {
          action.Start();
        }
        catch (Exception exception)
        {
          _logger.LogError("Action {name} failed with the following exception: {exception}", action.Name, exception);
        }
      }
      await Task.Delay(1000, stoppingToken);
    }
  }
}
