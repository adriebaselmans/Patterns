using System;
using System.Collections.Generic;
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
        PropertyChangedEventHandler _propertyChanged;

        IList<PropertyChangedEventHandler> _delegateList;

        public event PropertyChangedEventHandler PropertyChanged 
        {
            add 
            {
                _delegateList.Add(value);
                _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
                _delegateList.Remove(value);
            }
        }

        public WpfWeakProperty()
        {
            _delegateList = new List<PropertyChangedEventHandler>();
            var valuePropertyName = ReflectionHelper.GetPropertyName(() => Value);
            Subscribe((o, b) => OnPropertyChanged(valuePropertyName));
        }

        public override void Dispose()
        {
            foreach (var del in _delegateList)
            {
                _propertyChanged -= del;
            }

            _delegateList.Clear();

            base.Dispose();
        }

        public override sealed void Subscribe(Action<object, T> action)
        {
            base.Subscribe(action);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_propertyChanged != null) _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}