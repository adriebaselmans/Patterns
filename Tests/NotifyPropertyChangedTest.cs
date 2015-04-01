using System.ComponentModel;
using NUnit.Framework;
using Observer;

namespace Tests
{
    /// <summary>
    /// Example of a Model class
    /// </summary>
    public class Model
    {
        public WeakProperty<bool> MyWeakBooleanProperty = new WeakProperty<bool>();
    }


    /// <summary>
    /// Example ViewModel class, which exposes a property of a model to the view using a 'proxy' property.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Model _model;
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel(Model model)
        {
            _model = model;

            // The ViewModel should subscribe to notifications of the Model's weak property (MyWeakBooleanProperty) and notify the View 
            // using the INotifyPropertyChanged interface, via the ViewModels 'proxy' property.
            model.MyWeakBooleanProperty.Subscribe((o, b) => OnPropertyChanged(ReflectionHelper.GetPropertyName(() => MyProperty)));
        }

        public bool MyProperty
        {
            get { return _model.MyWeakBooleanProperty.Value; }
            set { _model.MyWeakBooleanProperty.Value = value; /*No OnPropertyChanged here, this is handled via the Model via the subscription to the Model's property in the constructor.*/ }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Example of use in an MVVM application.
    /// 
    /// In an MVVM application, normal events are typically used in the Model classes. This test illustrates how
    /// we can still expose the WeakProperty in the Model via the View by using a 'proxy' property and subscription.
    /// <see cref="ViewModel"/> and <see cref="Model"/>
    /// </summary>
    [TestFixture]
    public class NotifyPropertyChangedTest
    {
        [Test]
        public void GivenWeakProperty_WhenUsedInViewModel_PropertyNotifiesView()
        {
            var model = new Model();
            var viewModel = new ViewModel(model);

            int notificationCount = 0;
            viewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName.Equals(ReflectionHelper.GetPropertyName(() => viewModel.MyProperty))) notificationCount++;
                };

            Assert.AreEqual(0, notificationCount);

            model.MyWeakBooleanProperty.Value = !model.MyWeakBooleanProperty.Value;
            Assert.AreEqual(1, notificationCount);

            viewModel.MyProperty = !viewModel.MyProperty;
            Assert.AreEqual(2, notificationCount);
        }
    }
}