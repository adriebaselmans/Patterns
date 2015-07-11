using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Observer
{
    public class Property<T> : IObservable<T>, IDisposable where T : new()
    {
        private T _t;
        private readonly IList<IObserver<T>> _observers;

        public Property()
        {
            _t = new T();
            _observers = new List<IObserver<T>>();
        }

        public virtual void Dispose()
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

        public virtual void SubscribeThrottled(Action<object, T> action, int maxEventFrequencyInHz)
        {
            _observers.Add(new ThrottledObserver<T>(action, maxEventFrequencyInHz));
        }

        public void UnSubscribe(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        private void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Notify(this, _t);
            }
        }

        public T Value
        { 
            get { return _t; }
            set
            {
                if (!_t.Equals(value))
                {
                    _t = value;
                    Notify();
                }
            }
        }
    }
}