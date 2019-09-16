using System.Threading;

namespace HomeIrrigation.Sprinkler.Service
{
    public interface ITimer
    {
        bool Change(int dueTime, int period);
        event TimerCallback Elapsed;
    }
}
