using System;

namespace Observer
{
    internal class Program
    {
        public class MyClassWithObservables
        {
            public Property<bool> MyBoolean = new Property<bool>();
            public WeakProperty<double> MyWeakDouble = new WeakProperty<double>();  

            public Event<EventArgs> MyEvent = new Event<EventArgs>();
            public WeakEvent<EventArgs> MyWeakEvent = new WeakEvent<EventArgs>(); 
        }

        public class MyRefcountedClass
        {
            public MyRefcountedClass()
            {
                Console.WriteLine("Created instance of {0}, hash={1}", GetType(), GetHashCode());
            }

            public void SomeCallback<T>(object sender, T arg)
            {
                Console.WriteLine("Sender={0}, argument={1}", sender, arg);
            }

            ~MyRefcountedClass()
            {
                Console.WriteLine("Destroyed instance of {0}, hash={1}", GetType(), GetHashCode());
            }
        }

        public static void Main()
        {
            TestProperty();

            TestWeakProperty();

            TestEvent();

            TestWeakEvent();

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.ReadLine();
        }

        private static void TestProperty()
        {
            Console.WriteLine("--- Test Property ---");

            var myRefcountedClass1 = new MyRefcountedClass();
            var myRefcountedClass2 = new MyRefcountedClass();

            var myClass = new MyClassWithObservables();
            myClass.MyBoolean.Subscribe(new Observer<bool>(myRefcountedClass1.SomeCallback));
            myClass.MyBoolean.Subscribe(new WeakObserver<bool>(myRefcountedClass2.SomeCallback));

            myClass.MyBoolean.Value = true;
            myClass.MyBoolean.Value = false;
            myClass.MyWeakDouble.Value = 10.0f;

            myRefcountedClass1 = null;
            myRefcountedClass2 = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Disposing Property");
            myClass.MyBoolean.Dispose();
            myClass.MyBoolean = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        private static void TestWeakProperty()
        {
            Console.WriteLine("--- Test Weak Property ---");

            var myRefcountedClass1 = new MyRefcountedClass();

            var myClass = new MyClassWithObservables();
            myClass.MyWeakDouble.Subscribe(myRefcountedClass1.SomeCallback);

            myClass.MyWeakDouble.Value = 10.0f;

            myRefcountedClass1 = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Disposing Weak Property");
            myClass.MyWeakDouble.Dispose();
            myClass.MyWeakDouble = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        private static void TestEvent()
        {
            Console.WriteLine("--- Test Event ---");

            var myRefcountedClass1 = new MyRefcountedClass();
            var myRefcountedClass2 = new MyRefcountedClass();

            var myClass = new MyClassWithObservables();

            myClass.MyEvent.Subscribe(new Observer<EventArgs>(myRefcountedClass1.SomeCallback));
            myClass.MyEvent.Subscribe(new WeakObserver<EventArgs>(myRefcountedClass2.SomeCallback));
            myClass.MyEvent.Notify(new EventArgs());

            myRefcountedClass1 = null;
            myRefcountedClass2 = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Disposing Event");
            myClass.MyEvent.Dispose();
            myClass.MyEvent = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }

        private static void TestWeakEvent()
        {
            Console.WriteLine("--- Test Weak Event ---");

            var myRefcountedClass1 = new MyRefcountedClass();

            var myClass = new MyClassWithObservables();

            myClass.MyWeakEvent.Subscribe(new Observer<EventArgs>(myRefcountedClass1.SomeCallback));
            myClass.MyWeakEvent.Notify(new EventArgs());

            myRefcountedClass1 = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Disposing Weak Event");
            myClass.MyWeakEvent.Dispose();
            myClass.MyWeakEvent = null;

            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }
}