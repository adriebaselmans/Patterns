using System;
using System.ComponentModel;

namespace Observer
{
    /// <summary>
    /// This is a 'convenience' class representing a WeakProperty exposed directly via a ViewModel implementing INotifyPropertyChanged.
    /// Basically it offers the functionality of a WeakProperty but bindable directly to a WPF View (UI layer).
    /// 
    /// The drawback is that a simple property now actually is a ViewModel is itself.
    /// 
    /// Example in WPF XAML context:
    /// <CheckBox IsChecked="{Binding MyWpfWeakBooleanProperty.Value}"/>
    /// 
    /// Example in ViewModel context:
    /// public class ViewModel : INotifyPropertyChanged
    /// {
    ///     WpfWeakProperty<bool> MyWpfWeakBooleanProperty = new WpfWeakProperty<bool>();
    /// 
    ///     .... MyWpfWeakBooleanProperty.Value = ....
    /// 
    ///     .... INotifyPropertyChanged ....
    /// }
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WpfWeakProperty<T> : WeakProperty<T>, INotifyPropertyChanged where T : new()
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public WpfWeakProperty()
        {
            var valuePropertyName = ReflectionHelper.GetPropertyName(() => Value);
            Subscribe((o, b) => OnPropertyChanged(valuePropertyName));
        }

        public override sealed void Subscribe(Action<object, T> action)
        {
            base.Subscribe(action);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {        
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}