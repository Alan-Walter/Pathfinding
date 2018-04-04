using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour, ISelectObject {

    private Color SelectColor = Color.cyan;
    private Color DeselectColor = Color.white;
    private Renderer render;

    private TargetPoint targetPoint;

    // Use this for initialization
    void Start () {
        UnitManager.Instance.AddUnit(this);
        render = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnDestroy()
    {
        if(UnitManager.Instance != null)
            UnitManager.Instance.DeleteUnit(this);
    }

    public void OnSelectObject()    {
        render.material.color = SelectColor;
    }

    public void OnDeselectObject()  {
        render.material.color = DeselectColor;
    }

    public void OnSetTarget(TargetPoint point)
    {
        if(targetPoint != null)
            targetPoint.DeleteLink();
        targetPoint = point;
        targetPoint.AddLink();
    }
}
