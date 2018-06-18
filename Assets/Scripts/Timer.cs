using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private TimerStates timerState = TimerStates.Stopped;
    public float CallTime { get; set; }

    public delegate void Elapsed();
    public Elapsed onElapsed;

    // Update is called once per frame
    void Update () {
        if (timerState != TimerStates.Active) return;
        CallTime -= Time.deltaTime;
        if (CallTime < 0)
        {
            onElapsed.BeginInvoke(null, null);
            timerState = TimerStates.Ended;
        }
	}

    public void StartTimer() {
        timerState = TimerStates.Active;
    }

    public void StopTimer() {
        timerState = TimerStates.Stopped;
    }

    public void SetTimer(float time)
    {
        this.CallTime = time;
        StartTimer();
    }

    private enum TimerStates {
        Stopped,
        Active,
        Ended
    }
}
