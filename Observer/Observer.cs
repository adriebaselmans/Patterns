using System;
using System.Timers;

namespace Observer
{
    public class Observer<T> : IObserver<T>
    {
        private readonly Action<object, T> _action;

        public Observer(Action<object, T> action)
        {
            _action = action;
        }

        public virtual void Notify(object sender, T args)
        {
            if (_action != null) _action(sender, args);
        }
    }

    public class ThrottledObserver<T> : Observer<T>, IDisposable
    {
        private Timer _timer;
        private object _sender;
        private T _args;
        private bool _shouldNotify;

        private object _lockObject = new object();

        public ThrottledObserver(Action<object, T> action, TimeSpan throttleTime) 
            : base(action)
        {
            _timer = new Timer(throttleTime.TotalMilliseconds);
            _timer.AutoReset = true;
            _timer.Elapsed += OnThrottleTimeExpired;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Elapsed -= OnThrottleTimeExpired;
        }

        public override void Notify(object sender, T args)
        {
            lock (_lockObject)
            {
                _shouldNotify = true;
                _sender = sender;
                _args = args;
            }
        }

        void OnThrottleTimeExpired(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false; //avoid re-entrancy

            lock (_lockObject)
            {
                if (_shouldNotify)
                {
                    base.Notify(_sender, _args);
                    _shouldNotify = false;
                }
            }

            _timer.Enabled = true;
        }
    }
}