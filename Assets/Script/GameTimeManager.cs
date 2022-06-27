using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : SingletonComponent<GameTimeManager>
{
    // ƒQ[ƒ€“àŠÔ 0 ~ 1
    [SerializeField, Range(0, 1)] private float time = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        // 0 ~ 1‚ğ’´‚¦‚½‚è‰º‰ñ‚ç‚È‚¢‚æ‚¤‚ÉüŒ`•âŠÔ
        value = Mathf.Lerp(0, 1, value);
        time = value;
    }
}
