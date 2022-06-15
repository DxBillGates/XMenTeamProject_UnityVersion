using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogo : MonoBehaviour
{
    //スプライト
    [SerializeField] RectTransform ball;
    [SerializeField] RectTransform line;
    [SerializeField] RectTransform sonicS;
    [SerializeField] RectTransform sonicL;
    [SerializeField] RectTransform text;
    [SerializeField] RectTransform pressSpaceKey;
    [SerializeField] CanvasGroup pskAlpha;

    //定数
    //上に上がらせる値
    const float ADD_Y = 150;
    Vector3 POS_BALL_MOVE_START { get { return new Vector3(Screen.width + ball.sizeDelta.x / 2 , Screen.height - 257 - ball.sizeDelta.y / 2, 0); } }
    Vector3 POS_BALL_MOVE_END { get { return new Vector3(97 + ball.sizeDelta.x / 2, Screen.height - 257 - ball.sizeDelta.y / 2, 0); } }
    Vector3 POS_LINE_MOVE_START { get { return new Vector3(Screen.width + line.sizeDelta.x / 2, Screen.height - 423 - line.sizeDelta.y / 2, 0); } }
    Vector3 POS_LINE_MOVE_END { get { return new Vector3(301 + line.sizeDelta.x / 2, Screen.height - 423 - line.sizeDelta.y / 2, 0); } }
    Vector3 POS_SONIC_S_MOVE_START { get { return new Vector3(920 + sonicS.sizeDelta.x / 2, Screen.height - 246 - sonicS.sizeDelta.y / 2, 0); } }
    Vector3 POS_SONIC_S_MOVE_END { get { return new Vector3(1000 + sonicS.sizeDelta.x / 2, Screen.height - 246 - sonicS.sizeDelta.y / 2, 0); } }
    Vector3 POS_SONIC_L_MOVE_START { get { return new Vector3(920 + sonicL.sizeDelta.x / 2, Screen.height - 199 - sonicL.sizeDelta.y / 2, 0); } }
    Vector3 POS_SONIC_L_MOVE_END { get { return new Vector3(1050 + sonicL.sizeDelta.x / 2, Screen.height - 199 - sonicL.sizeDelta.y / 2, 0); } }
    Vector3 POS_TEXT_MOVE_START { get { return new Vector3(Screen.width + text.sizeDelta.x / 2, Screen.height - 279 - text.sizeDelta.y / 2, 0); } }
    Vector3 POS_TEXT_MOVE_END { get { return new Vector3(302 + text.sizeDelta.x / 2, Screen.height - 279 - text.sizeDelta.y / 2, 0); } }
    Vector3 POS_PSK_MOVE_START { get { return new Vector3(Screen.width / 2, Screen.height / 2, 0); } }
    Vector3 POS_PSK_MOVE_END { get { return new Vector3(Screen.width / 2, Screen.height / 2 - ADD_Y, 0); } }

    //タイマー
    float timer = -0.5f;
    const float TIME_LIMIT_TO_CENTER = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        timer = -0.5f;
        ball.transform.position = POS_BALL_MOVE_START;
        line.transform.position = POS_LINE_MOVE_START;
        sonicS.transform.position = new Vector3(Screen.width + 1000, 0, 0);        //初期は不可視
        sonicL.transform.position = new Vector3(Screen.width + 1000, 0, 0);        //初期は不可視
        text.transform.position = POS_TEXT_MOVE_START;
        pressSpaceKey.transform.position = new Vector3(Screen.width + 1000, 0, 0); //初期は不可視
        pskAlpha.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー更新
        timer += Time.deltaTime;

        //各種スプライト移動
        //中央へ
        if (timer < TIME_LIMIT_TO_CENTER)
        {
            //ボール
            ball.transform.position = new Vector3(
                EaseInExpo(POS_BALL_MOVE_START.x, POS_BALL_MOVE_END.x, timer / 0.5f),
                EaseInExpo(POS_BALL_MOVE_START.y, POS_BALL_MOVE_END.y, timer / 0.5f),
                EaseInExpo(POS_BALL_MOVE_START.z, POS_BALL_MOVE_END.z, timer / 0.5f)
                );

            //線
            line.transform.position = new Vector3(
                EaseInExpo(POS_LINE_MOVE_START.x, POS_LINE_MOVE_END.x, timer / 0.5f),
                EaseInExpo(POS_LINE_MOVE_START.y, POS_LINE_MOVE_END.y, timer / 0.5f),
                EaseInExpo(POS_LINE_MOVE_START.z, POS_LINE_MOVE_END.z, timer / 0.5f)
                );

            //テキスト
            text.transform.position = new Vector3(
                EaseInQuart(POS_TEXT_MOVE_START.x, POS_TEXT_MOVE_END.x, (timer - 0.1f) / 0.5f),
                EaseInQuart(POS_TEXT_MOVE_START.y, POS_TEXT_MOVE_END.y, (timer - 0.1f) / 0.5f),
                EaseInQuart(POS_TEXT_MOVE_START.z, POS_TEXT_MOVE_END.z, (timer - 0.1f) / 0.5f)
                );


            if (timer >= 0.5f)
            {
                //ソニックブームS
                sonicS.transform.position = new Vector3(
                    EaseOutExpo(POS_SONIC_S_MOVE_START.x, POS_SONIC_S_MOVE_END.x, (timer - 0.5f) / 0.5f),
                    EaseOutExpo(POS_SONIC_S_MOVE_START.y, POS_SONIC_S_MOVE_END.y, (timer - 0.5f) / 0.5f),
                    EaseOutExpo(POS_SONIC_S_MOVE_START.z, POS_SONIC_S_MOVE_END.z, (timer - 0.5f) / 0.5f)
                    );

                //ソニックブームL
                sonicL.transform.position = new Vector3(
                    EaseOutExpo(POS_SONIC_L_MOVE_START.x, POS_SONIC_L_MOVE_END.x, (timer - 0.5f) / 0.75f),
                    EaseOutExpo(POS_SONIC_L_MOVE_START.y, POS_SONIC_L_MOVE_END.y, (timer - 0.5f) / 0.75f),
                    EaseOutExpo(POS_SONIC_L_MOVE_START.z, POS_SONIC_L_MOVE_END.z, (timer - 0.5f) / 0.75f)
                    );
            }
        }
        //全て中央から上に
        else
        {
            //ボール
            ball.transform.position = new Vector3(
                EaseInOutCubic(POS_BALL_MOVE_END.x, POS_BALL_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_BALL_MOVE_END.y, POS_BALL_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_BALL_MOVE_END.z, POS_BALL_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //線
            line.transform.position = new Vector3(
                EaseInOutCubic(POS_LINE_MOVE_END.x, POS_LINE_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_LINE_MOVE_END.y, POS_LINE_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_LINE_MOVE_END.z, POS_LINE_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //テキスト
            text.transform.position = new Vector3(
                EaseInOutCubic(POS_TEXT_MOVE_END.x, POS_TEXT_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_TEXT_MOVE_END.y, POS_TEXT_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_TEXT_MOVE_END.z, POS_TEXT_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //ソニックブームS
            sonicS.transform.position = new Vector3(
                EaseInOutCubic(POS_SONIC_S_MOVE_END.x, POS_SONIC_S_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_S_MOVE_END.y, POS_SONIC_S_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_S_MOVE_END.z, POS_SONIC_S_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //ソニックブームL
            sonicL.transform.position = new Vector3(
                EaseInOutCubic(POS_SONIC_L_MOVE_END.x, POS_SONIC_L_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_L_MOVE_END.y, POS_SONIC_L_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_L_MOVE_END.z, POS_SONIC_L_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //Press Space Key
            pressSpaceKey.transform.position = new Vector3(
                EaseInOutCubic(POS_PSK_MOVE_START.x, POS_PSK_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_PSK_MOVE_START.y, POS_PSK_MOVE_END.y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_PSK_MOVE_START.z, POS_PSK_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //透明度
            pskAlpha.alpha = EaseInOutCubic(0, 1, timer - TIME_LIMIT_TO_CENTER);
        }
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

    float EaseInExpo(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
        float a = e - s;
        v = s + a * v;

        return v;
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
    float EaseInQuart(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t * t * t * t;
        float a = e - s;
        v = s + a * v;

        return v;
    }

    float EaseInOutCubic(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
