using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        Unit unit = other.gameObject.GetComponent<Unit>();
        TipsControls.Instance.SetTipsText("Победил игрок: " + unit.Player);
        GameParams.GamePlayState = GamePlayState.End;
    }
}
