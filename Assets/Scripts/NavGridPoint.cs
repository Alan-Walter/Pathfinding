using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NavGridPoint : IComparable, IEquatable<NavGridPoint>
{
    public Vector2Int Position;
    public int order;
    public float distance;
    public float Weight
    {
        get
        {
            return order + distance;
        }
    }
    public NavGridPoint oldPoint;

    public int CompareTo(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType()) return -1;
        NavGridPoint b = (NavGridPoint)obj;
        if (this.Weight < b.Weight) return 1;
        else if (this.Weight == b.Weight) return 0;
        return -1;
    }

    public NavGridPoint(Vector2Int pos, int order)
    {
        this.Position = pos;
        this.order = order;
        distance = 0;
        oldPoint = null;
    }

    public override bool Equals(object obj)
    {
        NavGridPoint temp = (NavGridPoint)obj;
        return this.Equals(temp);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool Equals(NavGridPoint other)
    {
        if (this.Position == other.Position) return true;
        return false;
    }
}
