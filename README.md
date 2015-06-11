Events
----------------------------------------------------------------------------------------------------------------------
Events can often be an cause of memory leaks because developers forget to unsubscibe.
In situations where event performance (how long it takes before the callback of an event subscription is called) is of
less importance, we rather use a event mechanism which prevents us from making mistakes related to memory management.

The WeakEvent in this project exactly does this. The Event class is NOT  memory leak safe. The WeakEvent class is memory leak safe.

Both implementations support event throttling.

For more information see the tests in the project.

Example:
            
            public Event<EventArgs> MyEvent = new Event<EventArgs>(); 
            MyEvent.Subscribe(...callback...)
            MyEvent.SubscribeThottled(...callback..., maxEventFrequencyInHz)
            MyEvent.Notify()
            
            public WeakEvent<EventArgs> MyWeakEvent = new WeakEvent<EventArgs>();  
            MyWeakEvent.Subscribe(...callback...)
            MyWeakEvent.SubscribeThottled(...callback..., maxEventFrequencyInHz)
            MyWeakEvent.Notify()
            
Properties
----------------------------------------------------------------------------------------------------------------------
The WPF Framework has the INotifyPropertyChanged mechanism to notify to the UI layer that a property has changed.
The Property (and it's 'memory-leak safe' counterpart WeakProperty) class offers a similar programming model. 
Note that the WeakProperty also has the same performance penalty as the WeakEvent. 

Notes:

The Property class is NOT  memory leak safe, the WeakProperty class is memory leak safe. 

Both implementations support event throttling.

For more information see the tests in the project.

Example:

            public Property<bool> MyBoolean = new Property<bool>();
            MyBoolean.Subscribe(...callback...)
            MyBoolean.SubscribeThottled(...callback..., maxEventFrequencyInHz)
            MyBoolean.Value = ... //triggers notify if value changed
            
            public WeakProperty<double> MyWeakDouble = new WeakProperty<double>();  
            MyWeakDouble.Subscribe(...callback...)
            MyWeakDouble.SubscribeThottled(...callback..., maxEventFrequencyInHz)
            MyWeakDouble.Value = ... //triggers notify if value changed

