using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScene : MonoBehaviour
{
    [SerializeField] RectTransform square;
    [SerializeField] RectTransform cd3; //�J�E���g�_�E��3
    [SerializeField] RectTransform cd2; //�J�E���g�_�E��2
    [SerializeField] RectTransform cd1; //�J�E���g�_�E��1
    [SerializeField] RectTransform cdGO; //�J�E���g�_�E��GO

    float timer = 0;
    float StartUpdateTime = 4.0f;
    Vector3 blackSquareInitScale = new Vector3((float)Screen.width / 100 + 1, (float)Screen.height / 25, 1);
    Vector3 blackSquareScale = new Vector3(1, 1, 1);
    static bool isGameStart = false;//���o�I�������

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        square.transform.position = new Vector3(Screen.width / 2, Screen.height / 2,0);
        square.transform.localScale = blackSquareInitScale;
        blackSquareScale = blackSquareInitScale;
        isGameStart = false;
        //�Q�[�����Ԗ߂�
        GameTimeManager.GetInstance().SetTime(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.GetInstance().IsPause() == true) return;
        if (isEnd == true) return;

        GameTimeManager.GetInstance().SetTime(0);

        //�^�C�}�[�X�V
        if (PauseManager.IsPause() == false)
        {
            timer += Time.deltaTime;
        }

        if (timer >= StartUpdateTime && isGameStart == false) {
            //�Q�[�����Ԗ߂�
            GameTimeManager.GetInstance().SetTime(1);
            isGameStart = true;
        }

        blackSquareScale = new Vector3(
            blackSquareInitScale.x,
            EaseOutExpo(blackSquareInitScale.y, 0, timer / 0.5f), 
            blackSquareInitScale.z
            );

        square.transform.localScale = blackSquareScale;

        //�J�E���g�_�E���X�v���C�g�S���A�T�C������Ă�����J�E���g�_�E���\��
        if (cd3 != null && cd2 != null && cd1 != null && cdGO != null)
        {
            UpdateCountDown();
        }
    }

    void UpdateCountDown()
    {
        //�J�E���g�_�E���O
        if (timer < 1)
        {
            cd3.transform.position = new Vector3(Screen.width + cd3.sizeDelta.x * 2, Screen.height / 2);
            cd2.transform.position = new Vector3(Screen.width + cd2.sizeDelta.x * 2, Screen.height / 2);
            cd1.transform.position = new Vector3(Screen.width + cd1.sizeDelta.x * 2, Screen.height / 2);
            cdGO.transform.position = new Vector3(Screen.width + cdGO.sizeDelta.x * 2, Screen.height / 2);
        }
        else if (timer < 2)
        {
            if (timer < 1.5f)
            {
                cd3.transform.position = new Vector3(
                    EaseOutExpo(Screen.width + cd3.sizeDelta.x * 2, Screen.width / 2, (timer - 1) / 0.5f),
                    Screen.height / 2
                    );
            }
            else
            {
                cd3.transform.position = new Vector3(
                    EaseInExpo(Screen.width / 2, -cd3.sizeDelta.x * 2, (timer - 1.5f) / 0.5f),
                    Screen.height / 2
                    );
            }
        }
        else if (timer < 3)
        {
            if (timer < 2.5f)
            {
                cd2.transform.position = new Vector3(
                    EaseOutExpo(Screen.width + cd2.sizeDelta.x * 2, Screen.width / 2, (timer - 2) / 0.5f),
                    Screen.height / 2
                    );
            }
            else
            {
                cd2.transform.position = new Vector3(
                    EaseInExpo(Screen.width / 2, -cd2.sizeDelta.x * 2, (timer - 2.5f) / 0.5f),
                    Screen.height / 2
                    );
            }
        }
        else if (timer < 4)
        {
            if (timer < 3.5f)
            {
                cd1.transform.position = new Vector3(
                    EaseOutExpo(Screen.width + cd1.sizeDelta.x * 2, Screen.width / 2, (timer - 3) / 0.5f),
                    Screen.height / 2
                    );
            }
            else
            {
                cd1.transform.position = new Vector3(
                    EaseInExpo(Screen.width / 2, -cd1.sizeDelta.x * 2, (timer - 3.5f) / 0.5f),
                    Screen.height / 2
                    );
            }
        }
        else if (timer < 5)
        {
            if (timer < 4.25f)
            {
                cdGO.transform.position = new Vector3(
                    EaseOutExpo(Screen.width + cdGO.sizeDelta.x * 2, Screen.width / 2, (timer - 4) / 0.25f),
                    Screen.height / 2
                    );
            }
            else
            {
                cdGO.transform.position = new Vector3(
                    EaseInExpo(Screen.width / 2, -cdGO.sizeDelta.x * 2, (timer - 4.25f) / 0.25f),
                    Screen.height / 2
                    );
            }
        }
    }

    //�ق�Ƃ͓����֐�2�����̗ǂ��Ȃ�����...
    float EaseOutExpo(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * t);
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

    public static bool IsGameStart()
    {
        return isGameStart;
    }
}
