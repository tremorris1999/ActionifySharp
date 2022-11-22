using System;
using System.Threading.Tasks;

namespace ActionifySharp.Worker;

public class WorkerAction
{
  private DateTime _nextExecution = DateTime.Now;
  public string Name { get; }
  public int Interval { get; }
  public DateTime NextExecution { get => _nextExecution; }
  public Action Action { get; }
  public WorkerAction(string name, int interval, Action action)
  {
    if (action is null)
      throw new ApplicationException("Action cannot be null");

    Name = name;
    Interval = interval;
    Action = action;
  }

  public async void Start()
  {
    await Task.Run(Action);
    _nextExecution = DateTime.Now.AddSeconds(Interval);
  }

}