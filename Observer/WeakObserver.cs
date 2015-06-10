using System;

namespace Observer
{
    public class WeakObserver<T> : IObserver<T>
    {
        private readonly WeakReference _weakReference;

        public WeakObserver(Action<object, T> action)
        {
            _weakReference = new WeakReference(action);
        }

        public virtual void Notify(object sender, T args)
        {
            if (_weakReference != null && _weakReference.IsAlive)
            {
                var action = _weakReference.Target as Action<object, T>;
                if (action != null) action(sender, args);
            }
        }
    }
}