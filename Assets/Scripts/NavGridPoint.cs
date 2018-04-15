using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class NavGridPoint : IComparable
{
    public int x, z, order;
    public float weight;
    public NavGridPoint oldPoint;

    public int CompareTo(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) return -1;
        NavGridPoint b = (NavGridPoint)obj;
        if (this.weight < b.weight) return 1;
        else if (this.weight == b.weight) return 0;
        return -1;
    }

    //public static bool operator <(NavGridPoint first, NavGridPoint second)
    //{
    //    return first.weight < second.weight;
    //}

    //public static bool operator >(NavGridPoint first, NavGridPoint second)
    //{
    //    return first.weight > second.weight;
    //}

    public NavGridPoint(int x, int z, int order)
    {
        this.x = x;
        this.z = z;
        this.order = order;
        weight = 0;
        oldPoint = null;
    }

    public override bool Equals(object obj)
    {
        NavGridPoint temp = (NavGridPoint)obj;
        if (this.x == temp.x && this.z == temp.z) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
