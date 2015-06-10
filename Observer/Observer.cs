using System;
using System.Threading;

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
}