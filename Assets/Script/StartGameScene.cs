using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScene : MonoBehaviour
{
    [SerializeField] RectTransform square;
    float timer = 0;
    Vector3 blackSquareInitScale = new Vector3((float)Screen.width / 100 + 1, (float)Screen.height / 25, 1);
    Vector3 blackSquareScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        square.transform.position = new Vector3(Screen.width / 2,Screen.height / 2,0);
        square.transform.localScale = blackSquareInitScale;
        blackSquareScale = blackSquareInitScale;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー更新
        timer += Time.deltaTime;
        if (timer >= 0.5f) { timer = 0.5f; }

        blackSquareScale = new Vector3(
            blackSquareInitScale.x,
            EaseOutExpo(blackSquareInitScale.y, 0, timer / 0.5f), 
            blackSquareInitScale.z
            );

        square.transform.localScale = blackSquareScale;
    }

    //ほんとは同じ関数2つ書くの良くないけど...
    float EaseOutExpo(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * t);
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
