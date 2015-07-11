using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Observer
{
    public class WpfWeakProperty<T> : WeakProperty<T>, IViewModel where T : new()
    {
        public void Register(ViewModelBase viewModel, Expression<Func<object>> property)
        {
            Subscribe( new Action<object, T>((o, s) => 
            { 
                viewModel.RaisePropertyChanged(ReflectionHelper.GetPropertyName(property)); 
            }));   
        }
    }
}