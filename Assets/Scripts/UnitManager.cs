﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Класс управления юнитами
/// </summary>
public class UnitManager : MonoBehaviour {

    private List<Unit> unitList;

    public static UnitManager Instance { get; private set; }

    public int Count;

    void Awake() {
        Instance = this;
        unitList = new List<Unit>();
    }

    //  добавление юнита
    public void AddUnit(Unit unit) {
        unitList.Add(unit);
        Count = unitList.Count;
    }

    //  удаление юнита
    public void DeleteUnit(Unit unit) {
        unitList.Remove(unit);
    }

    //  получение списка юнитов
    public List<Unit> GetUnitList() {
        return unitList;
    }

    void OnGUI() {
        GUI.Label(new Rect(0, Screen.height - 60, 150, 20), string.Format("Unit count: {0}", unitList.Count));
    }

    //  обнуление количества шагов у юнитов
    public void ResetUnitStepCount() {
        for (int i = 0; i < unitList.Count; i++)
            unitList[i].ResetCount();
    }
}
