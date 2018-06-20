using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Класс выделения объектов на игровой сцене
/// </summary>
public class SelectGameObject : MonoBehaviour {

    public static SelectGameObject Instance { get; private set; }
    private List<ISelectObject> selectionList;
    private UnitManager unitManager;
    private Vector3 firstPoint;
    private bool isSelectingObjects = false;
    public GameObject targetPoint;

    private GuiRectangle border;

    public bool IsSelectingObjects
    {
        get
        {
            return isSelectingObjects;
        }
    }

    // Use this for initialization
    void Start () {
        Instance = this;
	    unitManager = UnitManager.Instance;
        selectionList = new List<ISelectObject>();
        border = new GuiRectangle(1, 1);
        border.SetColor(Color.yellow);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameParams.GamePlayState != GamePlayState.Play)
        {
            isSelectingObjects = false;
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = GetHitFromCursor();
            if(hit.transform != null)
            {
                if (hit.transform.tag == "Terrain")
                {
                    isSelectingObjects = true;
                    firstPoint = Input.mousePosition;
                }
                else if(hit.transform.tag == "Unit")
                {
                    ClearSelectObjects();
                    Unit unit= hit.transform.GetComponent<Unit>();
                    if (unit.Player == PlayerController.ActivePlayer)
                    {
                        selectionList.Add(unit);
                        unit.OnSelectObject();
                    }
                }
            }
        }
        else
        if(Input.GetMouseButtonDown(1) && selectionList.Count != 0)
        {
            RaycastHit hit = GetHitFromCursor();
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Terrain")
                {
                    GameObject target = Instantiate(targetPoint, hit.point, new Quaternion()) as GameObject;
                    TargetPoint point = target.GetComponent<TargetPoint>();
                    if (TerrainNavGrid.Instance.IsCellUsed(point.Position)) return;
                    selectionList.OrderBy(i => Vector2Int.Distance(i.GridPosition, point.Position)).ToList().ForEach(x => x.OnSetTarget(point));
                }
            }
        }
        
        if(isSelectingObjects && Input.GetMouseButtonUp(0))
        {
            ClearSelectObjects();
            SelectUnitsInRectangle(firstPoint, Input.mousePosition);
            isSelectingObjects = false;
        }
	}

    //  получение RaycastHit под курсором
    public static RaycastHit GetHitFromCursor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }

    //  снимает выделение всех объектов
    public void ClearSelectObjects() {
        selectionList.ForEach(x => x.OnDeselectObject());
        selectionList.Clear();
    }

    private void SelectUnitsInRectangle(Vector3 first, Vector3 second) {
        if (second.x < first.x)
            GlobalFunctions.Swap(ref first.x, ref second.x);
        if(second.y > first.y)
            GlobalFunctions.Swap(ref first.y, ref second.y);
        Rect rect = GetRectangle(first, second);
        foreach(Unit unit in unitManager.GetUnitList())
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            screenPos.y = Screen.height - screenPos.y;
            if (rect.Contains(screenPos) && unit.Player == PlayerController.ActivePlayer)
            {
                selectionList.Add(unit);
                unit.OnSelectObject();
            }
        }
    }

    private static Rect GetRectangle(Vector3 first, Vector3 second) {
        float width = second.x - first.x;
        float height = (Screen.height - second.y) - (Screen.height - first.y);
        return new Rect(first.x, Screen.height - first.y, width, height);
    }

    void OnGUI() {
        if(isSelectingObjects && GameParams.GamePlayState == GamePlayState.Play)
            border.DrawBorderTexture(GetRectangle(firstPoint, Input.mousePosition), new Color(255, 184, 65), GameConstants.BorderThickness);
        if (selectionList != null && selectionList.Count == 1)
        {
            Unit unit = selectionList[0] as Unit;
            if (unit == null || unit.PathImage == null) return;
            GUI.DrawTexture(new Rect(Screen.width - 200, Screen.height - 200, 200, 200), unit.PathImage.GetImage());
        }
    }
}
