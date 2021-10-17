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

        public BeforeAfterData(T before, T after)
        {
            this.before = before;
            this.after = after;
        }
    }

    public static IObservable<BeforeAfterData<T>> BeforeAfter<T>(this IObservable<T> observable)
    {
        return observable.Zip(observable.Skip(1), (T before, T after) => new BeforeAfterData<T>(before, after));
    }

    public static IObservable<BeforeAfterData<float>> ClampWhere(this IObservable<BeforeAfterData<float>> observable, float clampChecker)
    {
        return observable.Where(time =>
        {
            if (time.before < time.after)
            {
                return time.before < clampChecker && clampChecker <= time.after;
            }
            else
            {
                return clampChecker > time.before || clampChecker <= time.after;
            }
        });
    }
}
