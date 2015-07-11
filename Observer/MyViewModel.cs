using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public class MyViewModel : ViewModelBase
    {
        public WpfWeakProperty<bool> MyLocalProperty = new WpfWeakProperty<bool>();

        public MyViewModel()
        {
            MyLocalProperty.Register(this, () => MyLocalProperty);
        }
    }
}
