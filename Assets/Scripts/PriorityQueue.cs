using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> { 
    private List<T> priority = new List<T>();

    public int Count
    {
        get
        {
            return this.priority.Count;
        }
    }

    public void Add(T value)
    {
        priority.Add(value);
        int i = Count - 1;
        int parent = (i - 1) / 2;

        while (i > 0 && (priority[i] as IComparable).CompareTo(priority[parent]) == 1)
        {
            T temp = priority[i];
            priority[i] = priority[parent];
            priority[parent] = temp;
            i = parent;
            parent = (i - 1) / 2;
        }
    }

    public T GetMin()
    {
        T result = priority[0];
        priority[0] = priority[Count - 1];
        priority.RemoveAt(Count - 1);
        Heapify(0);
        return result;
    }

    private void Heapify(int i)
    {
        int left, right, j;
        while (2 * i + 1 < Count)    // heapSize — количество элементов в куче
        {
            left = 2 * i + 1;             // left — левый сын
            right = 2 * i + 2;            // right — правый сын
            j = left;
            if (right < Count && (priority[right] as IComparable).CompareTo(priority[left]) == 1)
                j = right;
            if((priority[i] as IComparable).CompareTo(priority[j]) >= 0)
                break;
            T temp = priority[i];
            priority[i] = priority[j];
            priority[j] = temp;
            i = j;
        }
    }
}
