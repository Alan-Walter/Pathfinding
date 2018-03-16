using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour, ISelectObject {

    private Color SelectColor = Color.cyan;
    private Color DeselectColor = Color.white;
    private Renderer render;
    
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
        UnitManager.Instance.DeleteUnit(this);
    }

    public void OnSelectObject()    {
        render.material.color = SelectColor;
    }

    public void OnDeselectObject()  {
        render.material.color = DeselectColor;
    }
}
