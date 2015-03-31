using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EventTest
    {
        [Test]
        public void GivenEvent_WhenNotifyIsCalled_ThenObserverIsNotified()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyEvent.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            myClassWithObservables.MyEvent.Notify(new EventArgs());

            Assert.AreEqual(1, myRefCountedClass.NumberOfCallbacks);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
        }

        [Test]
        public void GivenEvent_WhenObserverIsNulled_ThenObjectRemainsAlive()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyEvent.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(1, refCount.Count);

            myRefCountedClass = null;

            ForceGarbageCollection();

            Assert.AreEqual(1, refCount.Count);

            myClassWithObservables.MyEvent.Dispose();
            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);
        }

        [Test]
        public void GivenWeakEvent_WhenObserverIsNulled_ThenObjectDoesNotRemainAlive()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyWeakEvent.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(1, refCount.Count);

            myRefCountedClass = null;

            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);

            myClassWithObservables.MyWeakEvent.Dispose();
            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);
        }

        [Test]
        public void GivenEvent_MeasurePerformance()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyEvent.Subscribe(myRefCountedClass.SomeCallback);


            var eventArgs = new EventArgs();
            long tsStart = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1000000; i++)
            {
                myClassWithObservables.MyEvent.Notify(eventArgs);
            }
            long tsEnd = Stopwatch.GetTimestamp();

            double elapsedNs = (tsEnd - tsStart)*(1.0/Stopwatch.Frequency)*1000*1000;
            Assert.Pass("Average Event notification time is {0} nanoseconds", elapsedNs / 1000000.0);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
        }

        [Test]
        public void GivenWeakEvent_MeasurePerformance()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyWeakEvent.Subscribe(myRefCountedClass.SomeCallback);


            var eventArgs = new EventArgs();
            long tsStart = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1000000; i++)
            {
                myClassWithObservables.MyWeakEvent.Notify(eventArgs);
            }
            long tsEnd = Stopwatch.GetTimestamp();

            double elapsedNs = (tsEnd - tsStart) * (1.0 / Stopwatch.Frequency) * 1000 * 1000;
            Assert.Pass("Average Weak Event notification time is {0} nanoseconds", elapsedNs / 1000000.0);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
        }

        private static void ForceGarbageCollection()
        {
            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }
}
