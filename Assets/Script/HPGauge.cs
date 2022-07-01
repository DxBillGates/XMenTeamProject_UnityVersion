using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //テクすやーの番号
    int texNumber { get; set; }
    //外部からHPを変える用の変数
    public float hp { private get; set; }
    private float oldHp = 1;

    //HP表示に使用する変数 HP変動した時のみ更新
    private float hpDispBefore = 1;
    private float hpDispAfter = 1;

    //ゲージ変動タイマー
    const float TIMER_LIMIT = 0.25f;
    float timerDecreaseHP = TIMER_LIMIT;
    bool isStartTimerDecreaseHP = false;

    Slider hpSlider;

    // Start is cSalled before the first frame update
    void Start()
    {
        texNumber = 0;
        hp = 1.0f;
        timerDecreaseHP = 0.5f;

        hpSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        texNumber++;
        hpSlider.value = Lerp(hpDispBefore, hpDispAfter, timerDecreaseHP / TIMER_LIMIT);

        if (isStartTimerDecreaseHP)
        {
            timerDecreaseHP += Time.deltaTime;
            if (timerDecreaseHP > TIMER_LIMIT)
            {
                timerDecreaseHP = TIMER_LIMIT;
                isStartTimerDecreaseHP = false;
            }
        }

        //HP変動してたらタイマースタート
        if (oldHp != hp)
        {
            hpDispBefore = oldHp;
            hpDispAfter = hp;

            timerDecreaseHP = 0;
            isStartTimerDecreaseHP = true;
        }

        oldHp = hp;
    }

    float Lerp(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t;
        float a = e - s;
        v = s + a * v;

        return v;
    }

}
