# ActionifySharp
Small library written for the purpose of performing actions on set intervals within a service.

## Configuration
ActionifySharp is configurable (in theory) with any singleton service.
```cs
IHostBuilder builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services => 
{
    services.AddActionify();
});

/* "Actionify" here */

IHost host = builder.Build();
await host.RunAsync();

```
You can "Actionify" any action, function, method, etc through a lambda function as a parameter in the `Actionify()` extension method. `Actionify()` can be called several times when configuring your host.
```cs
host.Actionify(worker => new WorkerAction("WriteTime", 1, () => System.Diagnostics.Debug.WriteLine($"{DateTime.Now}")));

host.Actionify(worker => new WorkerAction("CountActions", 1, () => System.Diagnostics.Debug.WriteLine($"{worker.Actions.Count()}")));
```

Alternatively, you can "Actionify" any `IEnumerable<WorkerAction>` to configure many actions in one call.
```cs
host.Actionify(worker => new List<WorkerAction>()
{
  { new WorkerAction("WriteTime", 1, () => System.Diagnostics.Debug.WriteLine($"{DateTime.Now}")) },
  { new WorkerAction("CountActions", 1, () =>
    {
        System.Diagnostics.Debug.WriteLine($"{worker.Actions.Count()}");
    })
  }
});
```

## Constructor
The constructor for a `WorkerAction` has the following signature:

```cs
public WorkerAction(string name, int interval, Action action)
```

`name` is used for logging the execution of the action, maybe more to come.
`interval` is the time between executions in seconds. A value of 60 would run each minute, while 3600 would run each hour, etc.
`action` is the action delegate to be performed upon execution.

## Contribution
I'm always more than happy to accept any contributions from a well formed PR. Feel free to contribute!