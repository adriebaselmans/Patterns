using System;

namespace Observer
{
    public class WeakEvent<T> : Event<T> where T : new()
    {
        public override void Subscribe(Action<object, T> action)
        {
            base.Subscribe(new WeakObserver<T>(action));
        }
    }
}