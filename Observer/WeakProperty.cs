﻿using System;

namespace Observer
{
    public class WeakProperty<T> : Property<T> where T : new()
    {
        public override void Subscribe(Action<object, T> action)
        {
            base.Subscribe(new WeakObserver<T>(action));
        }
    }
}