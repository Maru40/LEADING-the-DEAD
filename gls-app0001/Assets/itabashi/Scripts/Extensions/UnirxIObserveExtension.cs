using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public static class UniRxIObservableExtension
{
    public struct BeforeAfterData<T>
    {
        public T before;
        public T after;

        public BeforeAfterData(T before,T after)
        {
            this.before = before;
            this.after = after;
        }
    }

    public static IObservable<BeforeAfterData<T>> BeforeAfter<T>(this IObservable<T> observable)
    {
        return observable.Zip(observable.Skip(1), (T before, T after) => new BeforeAfterData<T>(before, after));
    }
}
