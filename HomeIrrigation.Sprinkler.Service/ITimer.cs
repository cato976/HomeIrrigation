using System.Threading;

namespace HomeIrrigation.Sprinkler.Service
{
    public interface ITimer
    {
        System.Threading.Timer GetRealTimer();
        void SetRealTimer(System.Threading.Timer value);
        bool Change(int dueTime, int period);
        event TimerCallback Elapsed;
    }
}
