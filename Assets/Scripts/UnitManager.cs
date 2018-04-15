using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    private List<Unit> unitList;

    public static UnitManager Instance { get; private set; }

    public int Count;

    void Awake()
    {
        Instance = this;
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
        Count = unitList.Count;
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
