using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    //スプライト
    [SerializeField] RectTransform circle;
    [SerializeField] RectTransform square;
    //遷移先のシーン名
    [SerializeField] string nextSceneName;

    //演出タイマー
    float timer = 0;
    Vector3 blackSquareScale = new Vector3(1, 1, 1);
    float maxScaleX = (float)Screen.width / 100 + 1;
    float maxScaleY = (float)Screen.height / 25;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        circle.transform.position = new Vector3(Screen.width + circle.sizeDelta.x / 2, Screen.height / 2, 0);
        square.transform.position = new Vector3(Screen.width + square.sizeDelta.x / 2 + circle.sizeDelta.x / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //タイマーと切り替え演出更新
        //タイマー更新
        timer += Time.deltaTime;

        //丸を横へ
        circle.transform.position = new Vector3(
            EaseOutExpo(Screen.width + circle.sizeDelta.x / 2, -circle.sizeDelta.x / 2, timer / 0.5f),
            Screen.height / 2,
            0
            );

        //四角を横へ
        square.transform.position = new Vector3(
            EaseOutExpo(Screen.width + square.sizeDelta.x / 2 + circle.sizeDelta.x / 2, Screen.width / 2, timer / 0.5f),
            Screen.height / 2,
            0
            );

        //四角を横に拡大
        blackSquareScale.x = EaseOutExpo(1, maxScaleX, timer / 0.5f);

        //四角を縦に拡大
        if (timer >= 0.75f)
        {
            blackSquareScale.y = EaseOutExpo(1, maxScaleY, (timer - 0.75f) / 0.5f);
        }

        square.transform.localScale = blackSquareScale;
        

        if (timer >= 2)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

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
