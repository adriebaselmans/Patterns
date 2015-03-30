using System;
using Observer;

namespace Tests
{
    public class MyClassWithObservables
    {
        public Property<bool> MyBooleanProperty = new Property<bool>();
        public WeakProperty<bool> MyWeakBooleanProperty = new WeakProperty<bool>();

        public Event<EventArgs> MyEvent = new Event<EventArgs>();
        public WeakEvent<EventArgs> MyWeakEvent = new WeakEvent<EventArgs>(); 
    }
}