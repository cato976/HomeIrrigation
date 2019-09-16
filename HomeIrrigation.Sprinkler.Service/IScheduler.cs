using System;

namespace HomeIrrigation.Sprinkler.Service
{
    public interface IScheduler
    {
        event EventHandler Alarm;
        void Start();
        void Stop();
    }
}
