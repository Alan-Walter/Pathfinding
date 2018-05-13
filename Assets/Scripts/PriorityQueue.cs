using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>: ICollection<T> where T: IComparable { 
    private List<T> priority = new List<T>();

    public int Count
    {
        get
        {
            return this.priority.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            return false;
        }
    }

    public void Add(T value)
    {
        if (this.Contains(value))
            return;
        priority.Add(value);
        int i = Count - 1;
        int parent = (i - 1) / 2;

        while (i > 0 && priority[i].CompareTo(priority[parent]) == 1)
        {
            T temp = priority[i];
            priority[i] = priority[parent];
            priority[parent] = temp;
            i = parent;
            parent = (i - 1) / 2;
        }
    }

    public void Clear()
    {
        priority.Clear();
    }

    public bool Contains(T item)
    {
        return priority.Contains(item);
    }

    public T Find(T item)
    {
        return priority.Find(a=>a.Equals(item));
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        List<T> temp = array.ToList();
        temp.InsertRange(arrayIndex, this.priority);
        array = temp.ToArray();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return priority.GetEnumerator();
    }

    public T GetMin()
    {
        T result = priority[0];
        priority[0] = priority[Count - 1];
        priority.RemoveAt(Count - 1);
        Heapify(0);
        return result;
    }

    public bool Remove(T item)
    {
        if (!this.Contains(item)) return false;
        int index = priority.IndexOf(item);
        T temp = priority[0];
        priority[0] = priority[index];
        priority[index] = temp;
        GetMin();
        Heapify(0);
        return true;
    }

    public T Peek()
    {
        return priority.First();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Heapify(int i)
    {
        int left, right, j;
        while (2 * i + 1 < Count)    // heapSize — количество элементов в куче
        {
            left = 2 * i + 1;             // left — левый сын
            right = 2 * i + 2;            // right — правый сын
            j = left;
            if (right < Count && priority[right].CompareTo(priority[left]) == 1)
                j = right;
            if(priority[i].CompareTo(priority[j]) >= 0)
                break;
            T temp = priority[i];
            priority[i] = priority[j];
            priority[j] = temp;
            i = j;
        }
    }
}
