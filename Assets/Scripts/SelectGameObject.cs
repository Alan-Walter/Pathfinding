using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectGameObject : MonoBehaviour {

    private List<ISelectObject> selectionList;
    private UnitManager unitManager;
    private Vector3 firstPoint;
    private bool isSelectObject = false;

    public bool IsSelectObject
    {
        get
        {
            return isSelectObject;
        }
    }

    // Use this for initialization
    void Start () {
	    unitManager = UnitManager.Instance;
        selectionList = new List<ISelectObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameStates.gamePlayState != GamePlayState.Play) return;
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = GetHitFromCursor();
            if(hit.transform != null && (hit.transform.tag == "Unit" || hit.transform.tag == "Terrain"))
            {
                isSelectObject = true;
                firstPoint = Input.mousePosition;
            }
        }
        if(isSelectObject && Input.GetMouseButtonUp(0))
        {
            ClearSelectObjects();
            SelectUnitsInRectangle(firstPoint, Input.mousePosition);
            isSelectObject = false;
        }
	}

    private RaycastHit GetHitFromCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }

    private void ClearSelectObjects()
    {
        selectionList.ForEach(x => x.OnDeselectObject());
        selectionList.Clear();
    }

    private void SelectUnitsInRectangle(Vector3 first, Vector3 second)
    {
        first.z = Camera.main.transform.position.y;
        second.z = Camera.main.transform.position.y;
        Debug.Log(first.ToString() + " кек " + second.ToString());
    }
}
