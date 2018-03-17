using UnityEngine;
using System.Collections;

public static class GlobalFunctions  {
    public static void Swap<T>(ref T lhs, ref T rhs) {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}
