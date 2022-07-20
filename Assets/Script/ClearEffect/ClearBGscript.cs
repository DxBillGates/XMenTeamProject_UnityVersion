using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearBGscript : MonoBehaviour
{
    // クリア時の背景を入れる
    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject clearImage;

    [SerializeField] private GameObject player;

    private RectTransform imageRect;
    private float startTime;

    private bool easingStart = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        imageRect = bgImage.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // 走るエフェクト終了時かつ、イージング開始時
        if (player.GetComponent<ClearPlayerMove>().IsEffectEnd && !easingStart)
        {
            startTime = Time.time;
            easingStart = true;

            bgImage.GetComponent<Image>().enabled = true;
        }

        if (easingStart)
        {

            // 現在のイージング開始からのトータル開始時間
            float totalTime = Time.time - startTime;
            
            // timeRateが１のとき
            if (totalTime >= 1)
            {
                totalTime = 1;

                clearImage.GetComponent<Image>().enabled = true;
            }
            Vector2 easingSize = Easing(totalTime, new Vector2(0, 900), new Vector2(0, 0));

            // 画像の大きさ用
            //imageRect.sizeDelta = easingSize;
            imageRect.anchoredPosition = new Vector3(0, easingSize.y, 0);
        }
    }

    private Vector2 Easing(float timeRate, Vector2 min, Vector2 max)
    {
        Vector2 result;
        max -= min;

        result.x = max.x * timeRate * timeRate * timeRate * timeRate + min.x;
        result.y = max.y * timeRate * timeRate * timeRate * timeRate + min.y;

        return result;
    }
}
