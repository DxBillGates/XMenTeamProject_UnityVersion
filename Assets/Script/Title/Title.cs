using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    //�X�v���C�g
    [SerializeField] RectTransform ball;
    [SerializeField] RectTransform line;
    [SerializeField] RectTransform sonicS;
    [SerializeField] RectTransform sonicL;
    [SerializeField] RectTransform text;
    [SerializeField] RectTransform pressSpaceKey;
    //�����x�M�邽�߂�
    [SerializeField] CanvasGroup sonicSAlpha;
    [SerializeField] CanvasGroup sonicLAlpha;
    [SerializeField] CanvasGroup pskAlpha;
    //�V�[���`�F���W�I�u�W�F�N�g
    [SerializeField] GameObject sceneChange;

    //�萔
    //��ɏオ�点��l
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
    Vector3 POS_OUT_CANVAS { get { return new Vector3(Screen.width * 2, Screen.height * 2, 0); } }

    //�^�C�}�[
    float timer = -0.5f;
    const float TIME_START_SONIC = 0.5f;
    const float TIME_LIMIT_TO_CENTER = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        timer = -0.5f;
        ball.transform.position = POS_BALL_MOVE_START;
        line.transform.position = POS_LINE_MOVE_START;
        sonicS.transform.position = new Vector3(Screen.width + 1000, 0, 0);        //�����͕s��
        sonicL.transform.position = new Vector3(Screen.width + 1000, 0, 0);        //�����͕s��
        text.transform.position = POS_TEXT_MOVE_START;
        pressSpaceKey.transform.position = new Vector3(Screen.width + 1000, 0, 0); //�����͕s��
        pskAlpha.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�X�V
        timer += Time.deltaTime;

        //�^�C�}�[�����̐��̎��ɂ͕`�悵�Ȃ�
        if (timer < 0) {
            ball.transform.position = POS_OUT_CANVAS;
            line.transform.position = POS_OUT_CANVAS;
            sonicS.transform.position = POS_OUT_CANVAS;
            sonicL.transform.position = POS_OUT_CANVAS;
            text.transform.position = POS_OUT_CANVAS;
            pressSpaceKey.transform.position = POS_OUT_CANVAS;
            return; 
        }

        //�e��X�v���C�g�ړ�
        //������
        if (timer < TIME_LIMIT_TO_CENTER)
        {
            //�{�[��
            ball.transform.position = new Vector3(
                EaseOutBack(POS_BALL_MOVE_START.x, POS_BALL_MOVE_END.x, timer / 0.5f),
                EaseOutBack(POS_BALL_MOVE_START.y, POS_BALL_MOVE_END.y, timer / 0.5f),
                EaseOutBack(POS_BALL_MOVE_START.z, POS_BALL_MOVE_END.z, timer / 0.5f)
                );

            //��
            line.transform.position = new Vector3(
                EaseOutBack(POS_LINE_MOVE_START.x, POS_LINE_MOVE_END.x, timer / 0.5f),
                EaseOutBack(POS_LINE_MOVE_START.y, POS_LINE_MOVE_END.y, timer / 0.5f),
                EaseOutBack(POS_LINE_MOVE_START.z, POS_LINE_MOVE_END.z, timer / 0.5f)
                );

            //�e�L�X�g
            text.transform.position = new Vector3(
                EaseOutBack(POS_TEXT_MOVE_START.x, POS_TEXT_MOVE_END.x, (timer - 0.1f) / 0.5f),
                EaseOutBack(POS_TEXT_MOVE_START.y, POS_TEXT_MOVE_END.y, (timer - 0.1f) / 0.5f),
                EaseOutBack(POS_TEXT_MOVE_START.z, POS_TEXT_MOVE_END.z, (timer - 0.1f) / 0.5f)
                );


            if (timer >= TIME_START_SONIC)
            {
                //�\�j�b�N�u�[��S
                sonicS.transform.position = new Vector3(
                    EaseOutExpo(POS_SONIC_S_MOVE_START.x, POS_SONIC_S_MOVE_END.x, (timer - TIME_START_SONIC) / 0.5f),
                    EaseOutExpo(POS_SONIC_S_MOVE_START.y, POS_SONIC_S_MOVE_END.y, (timer - TIME_START_SONIC) / 0.5f),
                    EaseOutExpo(POS_SONIC_S_MOVE_START.z, POS_SONIC_S_MOVE_END.z, (timer - TIME_START_SONIC) / 0.5f)
                    );

                //�\�j�b�N�u�[��L
                sonicL.transform.position = new Vector3(
                    EaseOutExpo(POS_SONIC_L_MOVE_START.x, POS_SONIC_L_MOVE_END.x, (timer - TIME_START_SONIC) / 0.75f),
                    EaseOutExpo(POS_SONIC_L_MOVE_START.y, POS_SONIC_L_MOVE_END.y, (timer - TIME_START_SONIC) / 0.75f),
                    EaseOutExpo(POS_SONIC_L_MOVE_START.z, POS_SONIC_L_MOVE_END.z, (timer - TIME_START_SONIC) / 0.75f)
                    );

                //�����x
                sonicSAlpha.alpha = EaseInOutCubic(0, 1, (timer - TIME_START_SONIC) / 0.5f);
                sonicLAlpha.alpha = EaseInOutCubic(0, 1, (timer - TIME_START_SONIC) / 0.5f);
            }
        }
        //�S�Ē���������
        else
        {
            //�{�[��
            ball.transform.position = new Vector3(
                EaseInOutCubic(POS_BALL_MOVE_END.x, POS_BALL_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_BALL_MOVE_END.y, POS_BALL_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_BALL_MOVE_END.z, POS_BALL_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //��
            line.transform.position = new Vector3(
                EaseInOutCubic(POS_LINE_MOVE_END.x, POS_LINE_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_LINE_MOVE_END.y, POS_LINE_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_LINE_MOVE_END.z, POS_LINE_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //�e�L�X�g
            text.transform.position = new Vector3(
                EaseInOutCubic(POS_TEXT_MOVE_END.x, POS_TEXT_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_TEXT_MOVE_END.y, POS_TEXT_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_TEXT_MOVE_END.z, POS_TEXT_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //�\�j�b�N�u�[��S
            sonicS.transform.position = new Vector3(
                EaseInOutCubic(POS_SONIC_S_MOVE_END.x, POS_SONIC_S_MOVE_END.x, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_S_MOVE_END.y, POS_SONIC_S_MOVE_END.y + ADD_Y, timer - TIME_LIMIT_TO_CENTER),
                EaseInOutCubic(POS_SONIC_S_MOVE_END.z, POS_SONIC_S_MOVE_END.z, timer - TIME_LIMIT_TO_CENTER)
                );

            //�\�j�b�N�u�[��L
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

            //�����x
            pskAlpha.alpha = EaseInOutCubic(0, 1, timer - TIME_LIMIT_TO_CENTER);
        }

        //Space�L�[�ŊJ�n
        if (timer >= 2.5f && Input.GetButtonDown("PlayerAbility"))
        {
            sceneChange.SetActive(true);
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

    float EaseOutBack(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        const float c1 = 1.70158f;
        const float c3 = c1 + 1;

        float v = 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
