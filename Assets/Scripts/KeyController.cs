using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && GameParams.GamePlayState != GamePlayState.Menu)
        {
            SceneManager.LoadScene("menu");
        }
        if(Input.GetKeyDown(KeyCode.Pause))
        {
            GameParams.CameraState = GameParams.CameraState == CameraStates.Normal ? CameraStates.Freeze : CameraStates.Normal;
        }
    }
}
