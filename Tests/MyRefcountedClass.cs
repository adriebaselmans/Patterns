namespace Tests
{
    public class MyRefcountedClass
    {
        private readonly Counter _referenceCounter;
        public int NumberOfCallbacks { get; private set; }

        public MyRefcountedClass(Counter referenceCounter)
        {
            _referenceCounter = referenceCounter;
            referenceCounter.Count++;
        }

        public void SomeCallback<T>(object sender, T arg)
        {
            NumberOfCallbacks++;
        }

        ~MyRefcountedClass()
        {
            _referenceCounter.Count--;
        }
    }
}