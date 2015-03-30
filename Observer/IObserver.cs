namespace Observer
{
    public interface IObserver<in T> 
    {
        void Notify(object sender, T args);
    }
}