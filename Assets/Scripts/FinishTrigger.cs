using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за обработку входа в зону финиша юнита игрока
/// </summary>
public class FinishTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        Unit unit = other.gameObject.GetComponent<Unit>();  //  получение объекта Unit с gameObject 
        TipsControls.Instance.SetTipsText("Победил игрок: " + unit.Player);  //  вывод сообщения на экран
        GameParams.GamePlayState = GamePlayState.End;  //  состояние игры переходит в End
    }
}
