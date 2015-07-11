using System.ComponentModel;
using NUnit.Framework;
using Observer;
using System;

namespace Tests
{
    public class MyViewModel : ViewModelBase
    {
        public WpfWeakProperty<bool> MyLocalProperty = new WpfWeakProperty<bool>();

        public MyViewModel()
        {
            MyLocalProperty.Register(this, () => MyLocalProperty);
        }
    }

    [TestFixture]
    public class MyViewModelTest
    {
        [Test]
        public void TestMyBaseViewModel()
        {
            MyViewModel myViewModel = new MyViewModel();

            int notificationCount = 0;
            myViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(ReflectionHelper.GetPropertyName(() => myViewModel.MyLocalProperty))) notificationCount++;
            };

            Assert.AreEqual(0, notificationCount);

            myViewModel.MyLocalProperty.Value = !myViewModel.MyLocalProperty.Value;
            Assert.AreEqual(1, notificationCount);

            myViewModel.MyLocalProperty.Value = !myViewModel.MyLocalProperty.Value;
            Assert.AreEqual(2, notificationCount);
        }
    }
}