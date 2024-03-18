using System;
using System.Collections.Generic;

public class Pool<T> where T : class, IPoolable
{
    private object @lock = new object();
    private Stack<T> free = new Stack<T>();
    private HashSet<T> busy = new HashSet<T>();

    public T New(Func<T> constructor)
    {
        lock (@lock)
        {
            if (free.Count == 0)
            {
                free.Push(constructor());
            }

            var item = free.Pop();
            item.PoolNew();
            busy.Add(item);
            return item;
        }
    }

    public void Free(T item)
    {
        lock (@lock)
        {
            if (!busy.Remove(item))
            {
                throw new ArgumentException("The item to free is not in use by the pool.", nameof(item));
            }

            item.PoolFree();
            free.Push(item);
        }
    }
}
