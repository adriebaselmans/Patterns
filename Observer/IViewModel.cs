using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Observer
{
    public interface IViewModel
    {
        void Register(ViewModelBase viewModel, Expression<Func<object>> property);
    }
}
