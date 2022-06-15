using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScene : MonoBehaviour
{
    [SerializeField] RectTransform blackSquare;
    float timer = 0;
    Vector3 blackSquareInitScale = new Vector3(8, 15, 1);
    Vector3 blackSquareScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        blackSquare.transform.position = new Vector3(Screen.width / 2,Screen.height / 2,0);
        blackSquare.transform.localScale = blackSquareInitScale;
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

        blackSquare.transform.localScale = blackSquareScale;
    }

    //ほんとは同じ関数2つ書くの良くないけど...
    float EaseOutExpo(float s, float e, float t)
    {
        float v = t == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * t);
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
