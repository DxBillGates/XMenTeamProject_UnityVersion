using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : SingletonComponent<GameTimeManager>
{
    // ゲーム内時間 0 ~ 1
    [SerializeField, Range(0, 1)] private float time = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        // 0 ~ 1を超えたり下回らないように線形補間
        value = Mathf.Lerp(0, 1, value);
        time = value;
    }
}