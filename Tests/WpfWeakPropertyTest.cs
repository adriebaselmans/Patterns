using NUnit.Framework;
using Observer;

namespace Tests
{
    [TestFixture]
    public class WpfWeakPropertyTest
    {
        [Test]
        public void GivenWpfWeakProperty_WhenUsedInViewModel_PropertyNotifiesView()
        {
            var wpfPropertyInViewModel = new WpfWeakProperty<bool>();

            int notificationCount = 0;
            wpfPropertyInViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(ReflectionHelper.GetPropertyName(() => wpfPropertyInViewModel.Value))) notificationCount++;
            };

            Assert.AreEqual(0, notificationCount);

            wpfPropertyInViewModel.Value = !wpfPropertyInViewModel.Value;
            Assert.AreEqual(1, notificationCount);
        }
    }
}
