using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ListExtension
{
    public static T Dequeue<T>(this List<T> list)
    {
        var r = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return r;
    }
}
