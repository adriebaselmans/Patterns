using Observer;
using System;
using System.Threading;

public class WeakThrottledObserver<T> : WeakObserver<T>, IDisposable
{
    private Timer _timer;
    private object _sender;
    private T _args;
    private bool _shouldNotify;
    private bool _notifying;

    private object _lockObject = new object();

    public WeakThrottledObserver(Action<object, T> action, int maxEventFrequencyInHz)
        : base(action)
    {
        if (maxEventFrequencyInHz <= 0) throw new ArgumentOutOfRangeException();
        int msBetweenNotifies = 1000 / maxEventFrequencyInHz;
        _timer = new Timer(new TimerCallback(this.OnThrottleTimeExpired), null, 0, msBetweenNotifies);
    }

    public void Dispose()
    {
        _timer.Dispose();
        _timer = null;
    }

    public override void Notify(object sender, T args)
    {
        lock (_lockObject)
        {
            _sender = sender;
            _args = args;
            _shouldNotify = true;
        }
    }

    void OnThrottleTimeExpired(object state)
    {
        lock (_lockObject)
        {
            if (_shouldNotify && !_notifying)
            {
                _notifying = true;
                base.Notify(_sender, _args);
                _notifying = false;
                _shouldNotify = false;
            }
        }
    }
}
