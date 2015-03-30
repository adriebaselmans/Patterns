namespace Observer
{
    public interface IObservable<out T>
    {
        void Subscribe(IObserver<T> observer);
        void UnSubscribe(IObserver<T> observer);
    }
}
