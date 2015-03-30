using System;
using System.Collections.Generic;
using System.Linq;

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