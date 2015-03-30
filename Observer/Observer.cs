using System;

namespace Observer
{
    public class Observer<T> : IObserver<T>
    {
        private readonly Action<object, T> _action;

        public Observer(Action<object, T> action)
        {
            _action = action;
        }

        public void Notify(object sender, T args)
        {
            if (_action != null) _action(sender, args);
        }
    }
}