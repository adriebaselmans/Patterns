Events
----------------------------------------------------------------------------------------------------------------------
Events can often be an cause of memory leaks because developers forget to unsubscibe.
In situations where event performance (how long it takes before the callback of an event subscription is called) is of
less importance, we rather use a event mechanism which prevents us from making mistakes related to memory management.

The WeakEvent in this project exactly does this. The Event class is NOT  memory leak safe. The WeakEvent class is memory leak safe.
For more information see the tests in the project.

Example:
            
            public Event<EventArgs> MyEvent = new Event<EventArgs>(); 
            MyEvent.Subscribe(...callback...)
            MyEvent.Notify()
            
            public WeakEvent<EventArgs> MyWeakEvent = new WeakEvent<EventArgs>();  
            MyWeakEvent.Subscribe(...callback...)
            MyWeakEvent.Notify()
            
Properties
----------------------------------------------------------------------------------------------------------------------
The WPF Framework has the INotifyPropertyChanged mechanism to notify to the UI layer that a property has changed.
The Property (and it's 'memory-leak safe' counterpart WeakProperty) class offers a similar programming model. 
Note that the WeakProperty also has the same performance penalty as the WeakEvent. 

Notes:
In most cases, the events perform in the order of 1000 times per second. This is enough for normal application development, where 'realtime' response is of less importance. 

The Property class is NOT  memory leak safe, the WeakProperty class is memory leak safe. 
For more information see the tests in the project.

Example:

            public Property<bool> MyBoolean = new Property<bool>();
            MyBoolean.Subscribe(...callback...)
            MyBoolean.Value = ... //triggers notify if value changed
            
            public WeakProperty<double> MyWeakDouble = new WeakProperty<double>();  
            MyWeakDouble.Subscribe(...callback...)
            MyWeakDouble.Value = ... //triggers notify if value changed
