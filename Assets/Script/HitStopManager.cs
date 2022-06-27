using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopManager : SingletonComponent<HitStopManager>
{
    [SerializeField] private float hitStopTime;

    private bool isHitStop;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        isHitStop = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHitStop == false) return;
        if(time > hitStopTime)
        {
            isHitStop = false;
            time = 0;
            GameTimeManager.GetInstance().SetTime(1);
            return;
        }


        time += Time.deltaTime;
    }

    public void HitStop()
    {
        GameTimeManager.GetInstance().SetTime(0);
        isHitStop = true;
        time = 0;
    }
}
