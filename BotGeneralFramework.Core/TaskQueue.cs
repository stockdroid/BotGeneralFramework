using BotGeneralFramework.Interfaces.Core;

namespace BotGeneralFramework.Core;
public sealed class TaskQueue
{
  private readonly Queue<Task> _lowerPriorityTaskQueue = new();
  private readonly Queue<Task> _higherPriorityTaskQueue = new();
  private int _queueDelay;
  private int _queueMax;
  private Task? _currentTask;
  private Task _cycleTask = null!;
  private IApp App { get; set; } = null!;
  private readonly string _queueName;

  private void End() => _currentTask = null;
  private void Start(Task task) => _currentTask = task;

  private Task Cycle(Queue<Task> queue)
  {
    if (queue.Count == 0) { End(); return Task.CompletedTask; }
    _currentTask = queue.Dequeue();

    App.trigger($"{_queueName}.elementDequeued", new Dictionary<string, object?>()
    {
      { "queueCount", _lowerPriorityTaskQueue.Count + _higherPriorityTaskQueue.Count }
    });

    return _cycleTask = Task.Delay(_queueDelay).ContinueWith(
      (task) => _currentTask.Start()
    ).ContinueWith(
      (task) => Cycle(queue).Wait()
    );
  }

  private Task CycleHighPriority => Cycle(_higherPriorityTaskQueue);
  private Task CycleLowPriority => Cycle(_lowerPriorityTaskQueue);
  private Task Run()
  {
    if (_currentTask is not null) return _cycleTask;
    if (_higherPriorityTaskQueue.Count > 0) return CycleHighPriority;
    else return CycleLowPriority;
  }

  private Task Enqueue(Task task, Queue<Task> queue, int max)
  {
    if (queue.Count > max)
      return Task.FromException(new Exception("Too many requests"));
    queue.Enqueue(task);

    App.trigger($"{_queueName}.elementEnqueued", new Dictionary<string, object?>()
    {
      { "queueCount", _lowerPriorityTaskQueue.Count + _higherPriorityTaskQueue.Count }
    });

    Run();
    return Task.CompletedTask;
  }

  public Task EnqueueLowPriority(Task task) => Enqueue(task, _lowerPriorityTaskQueue, _queueMax);
  public Task EnqueueHighPriority(Task task) => Enqueue(task, _higherPriorityTaskQueue, 10);

  public void SetDelay(int delayInMS) => _queueDelay = delayInMS;
  public void Ready(IApp app)
  {
    App = app;
    App.trigger($"{_queueName}.ready");
  }

  public TaskQueue(string queueName, int delayInMS = 50, int queueMax = 50)
  {
    _queueDelay = delayInMS;
    _queueMax = queueMax;
    _queueName = queueName;
  }
}