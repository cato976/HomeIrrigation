using System.Threading;

namespace HomeIrrigation.Sprinkler.Service
{
    public class Timer : ITimer
    {
        public Timer()
        {
        }

        System.Threading.Timer RealTimer { get; set; }

        #region ITimer

        public System.Threading.Timer GetRealTimer()
        {
            return RealTimer;
        }

        public event TimerCallback Elapsed;

        public bool Change(int dueTime, int period)
        {
            return RealTimer.Change(dueTime, period);
        }

        System.Threading.Timer ITimer.GetRealTimer()
        {
            throw new System.NotImplementedException();
        }

        void ITimer.SetRealTimer(System.Threading.Timer value)
        {
            RealTimer = value;
        }

        #endregion ITimer
    }
}
