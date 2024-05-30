using System.Collections.Concurrent;

var timerByTerminalDictionary = new ConcurrentDictionary<int, Timer>();
var settingId = 1;
var timer = new Timer(DoWork, settingId, TimeSpan.FromMinutes(2), TimeSpan.Zero);

if (timerByTerminalDictionary.TryAdd(settingId, timer) == false)
{
    timer.Change(Timeout.Infinite, 0);
    timer.Dispose();
    timer = null;
}

Timer timerFromDictionary;

if (timerByTerminalDictionary.TryGetValue(settingId, out timerFromDictionary))
{
    var newTimer = new Timer(DoWork, settingId, TimeSpan.FromSeconds(10), TimeSpan.Zero);

    var isEqual = timer!.Equals(newTimer);

    if (timerByTerminalDictionary.TryUpdate(settingId, newTimer, timerFromDictionary))
    {
        isEqual = timer!.Equals(timerFromDictionary);

        timerFromDictionary.Change(Timeout.Infinite, 0);
        timerFromDictionary.Dispose();
        timerFromDictionary = null;
    }
}

async void DoWork(object? settingId)
{
    Console.WriteLine("DoWork");
   Task.CompletedTask.Wait(100);
}