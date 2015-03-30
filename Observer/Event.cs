using System;
using System.Collections.Generic;

namespace Observer
{
    public class Event<T> : IObservable<T>, IDisposable where T : new()
    {
        private readonly IList<IObserver<T>> _observers;

        public Event()
        {
            _observers = new List<IObserver<T>>();
        }

        public void Dispose()
        {
            _observers.Clear();
        }

        public void Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
        }

        public virtual void Subscribe(Action<object, T> action)
        {
            _observers.Add(new Observer<T>(action));
        }

        public void UnSubscribe(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(T args)
        {
            foreach (var observer in _observers)
            {
                observer.Notify(this, args);
            }
        }
    }
}