using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    private static UnitManager instance;
    private List<Unit> unitList;

    public static UnitManager Instance
    {
        get {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        unitList = new List<Unit>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void DeleteUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
}
