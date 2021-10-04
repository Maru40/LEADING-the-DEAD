using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QueueExtension
{
    public static bool Empty<T>(this Queue<T> queue)
    {
        return queue.Count == 0;
    }
}
