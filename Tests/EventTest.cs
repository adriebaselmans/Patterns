using System;
using System.Diagnostics;
using NUnit.Framework;
using Observer;
using System.Threading;

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

        [Test]
        public void GivenEventAndThrottledObserver_WhenNotifyIsCalled_ThenNotificationIsLimitedCloseToDesiredFrequency()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            const int desiredEventFrequencyInHz = 125;
            var myClassWithObservables = new MyClassWithObservables();

            var throttledObserver = new ThrottledObserver<EventArgs>(myRefCountedClass.SomeCallback, desiredEventFrequencyInHz);
            myClassWithObservables.MyEvent.Subscribe(throttledObserver);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            var sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 1000)
            {
                myClassWithObservables.MyEvent.Notify(new EventArgs());
            }
            sw.Stop();

            Assert.Greater(myRefCountedClass.NumberOfCallbacks, 0);
            Assert.Less(myRefCountedClass.NumberOfCallbacks, desiredEventFrequencyInHz);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
            throttledObserver.Dispose();
        }

        [Test]
        public void GivenEventAndWeakThrottledObserver_WhenNotifyIsCalled_ThenNotificationIsLimitedCloseToDesiredFrequency()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            const int desiredEventFrequencyInHz = 125;
            var myClassWithObservables = new MyClassWithObservables();

            var throttledObserver = new WeakThrottledObserver<EventArgs>(myRefCountedClass.SomeCallback, desiredEventFrequencyInHz);
            myClassWithObservables.MyEvent.Subscribe(throttledObserver);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            var sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 1000)
            {
                myClassWithObservables.MyEvent.Notify(new EventArgs());
            }
            sw.Stop();

            Assert.Greater(myRefCountedClass.NumberOfCallbacks, 0);
            Assert.LessOrEqual(myRefCountedClass.NumberOfCallbacks, desiredEventFrequencyInHz);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
            throttledObserver.Dispose();
        }

        private static void ForceGarbageCollection()
        {
            GC.Collect();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }
}
