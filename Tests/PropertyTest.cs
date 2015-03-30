using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PropertyTest
    {
        [Test]
        public void GivenProperty_WhenValueIsChanged_ThenObserverIsNotified()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            Assert.AreEqual(false, myClassWithObservables.MyBooleanProperty.Value);
            myClassWithObservables.MyBooleanProperty.Value = true;
      
            Assert.AreEqual(1, myRefCountedClass.NumberOfCallbacks);

            myRefCountedClass = null;
            myClassWithObservables.MyBooleanProperty.Dispose();
        }

        [Test]
        public void GivenProperty_WhenValueIsChanged_ThenObserverIsNotifiedOnlyWhenValueIsDifferent()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            Assert.AreEqual(false, myClassWithObservables.MyBooleanProperty.Value);
            myClassWithObservables.MyBooleanProperty.Value = true;
            myClassWithObservables.MyBooleanProperty.Value = true;
            myClassWithObservables.MyBooleanProperty.Value = false;

            Assert.AreEqual(2, myRefCountedClass.NumberOfCallbacks);

            myRefCountedClass = null;
            myClassWithObservables.MyBooleanProperty.Dispose();
        }

        [Test]
        public void GivenWeakProperty_WhenValueIsChanged_ThenObserverIsNotified()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyWeakBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(0, myRefCountedClass.NumberOfCallbacks);

            Assert.AreEqual(false, myClassWithObservables.MyWeakBooleanProperty.Value);
            myClassWithObservables.MyWeakBooleanProperty.Value = true;
            myClassWithObservables.MyWeakBooleanProperty.Value = false;
            myClassWithObservables.MyWeakBooleanProperty.Value = true;

            Assert.AreEqual(3, myRefCountedClass.NumberOfCallbacks);

            myRefCountedClass = null;
            myClassWithObservables.MyBooleanProperty.Dispose();
        }

        [Test]
        public void GivenProperty_WhenObserverIsNulled_ThenObjectRemainsAlive()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(1, refCount.Count);

            myRefCountedClass = null;

            ForceGarbageCollection();

            Assert.AreEqual(1, refCount.Count);

            myClassWithObservables.MyBooleanProperty.Dispose();
            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);
        }

        [Test]
        public void GivenWeakProperty_WhenObserverIsNulled_ThenObjectDoesNotRemainAlive()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyWeakBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);
            Assert.AreEqual(1, refCount.Count);

            myRefCountedClass = null;

            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);

            myClassWithObservables.MyBooleanProperty.Dispose();
            ForceGarbageCollection();

            Assert.AreEqual(0, refCount.Count);
        }

        [Test]
        public void GivenProperty_MeasurePerformance()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);


            long tsStart = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1000; i++)
            {
                myClassWithObservables.MyBooleanProperty.Value = !myClassWithObservables.MyBooleanProperty.Value;
            }
            long tsEnd = Stopwatch.GetTimestamp();

            double elapsedNs = (tsEnd - tsStart) * (1.0 / Stopwatch.Frequency) * 1000 * 1000;
            Assert.Pass("Average Property notification time is {0:0} nanoseconds", elapsedNs);

            myRefCountedClass = null;
            myClassWithObservables.MyEvent.Dispose();
        }

        [Test]
        public void GivenWeakProperty_MeasurePerformance()
        {
            var refCount = new Counter();
            var myRefCountedClass = new MyRefcountedClass(refCount);
            Assert.AreEqual(1, refCount.Count);

            var myClassWithObservables = new MyClassWithObservables();
            myClassWithObservables.MyWeakBooleanProperty.Subscribe(myRefCountedClass.SomeCallback);


            var eventArgs = new EventArgs();
            long tsStart = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1000; i++)
            {
                myClassWithObservables.MyWeakBooleanProperty.Value = !myClassWithObservables.MyWeakBooleanProperty.Value;
            }
            long tsEnd = Stopwatch.GetTimestamp();

            double elapsedNs = (tsEnd - tsStart) * (1.0 / Stopwatch.Frequency) * 1000 * 1000;
            Assert.Pass("Average Weak Property notification time is {0:0} nanoseconds", elapsedNs);

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