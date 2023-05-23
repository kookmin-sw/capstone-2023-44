using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer
{
    private float limitTime = 25.0f;

    public Timer(float initTime)
    {
        limitTime = initTime;
    }

    //for game logic
    public float GetTime()
    {
        return limitTime -= Time.deltaTime;
    }

    //for ui update
    public string GetlimitTime()
    {
        string time = string.Format("{0:0.#0}", limitTime -= Time.deltaTime);
        return time;
    }
}
