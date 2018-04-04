using UnityEngine;
using System.Collections;

public interface ISelectObject {
    void OnSelectObject();
    void OnDeselectObject();
    void OnSetTarget(TargetPoint point);
}
