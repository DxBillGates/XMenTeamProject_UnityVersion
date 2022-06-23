using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScene : MonoBehaviour
{
    [SerializeField] RectTransform square;
    [SerializeField] float StartUpdateTime = 2.0f;
    float timer = 0;
    Vector3 blackSquareInitScale = new Vector3((float)Screen.width / 100 + 1, (float)Screen.height / 25, 1);
    Vector3 blackSquareScale = new Vector3(1, 1, 1);
    bool isEnd = false;//���o�I�������

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        square.transform.position = new Vector3(Screen.width / 2,Screen.height / 2,0);
        square.transform.localScale = blackSquareInitScale;
        blackSquareScale = blackSquareInitScale;
        isEnd = false;
        //�Q�[�����Ԗ߂�
        GameTimeManager.GetInstance().SetTime(0);
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�X�V
        timer += Time.deltaTime;

        if (timer >= StartUpdateTime && isEnd == false) {
            timer = StartUpdateTime;
            //�Q�[�����Ԗ߂�
            GameTimeManager.GetInstance().SetTime(1);
            isEnd = true;
        }

        blackSquareScale = new Vector3(
            blackSquareInitScale.x,
            EaseOutExpo(blackSquareInitScale.y, 0, timer / 0.5f), 
            blackSquareInitScale.z
            );

        square.transform.localScale = blackSquareScale;
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
}
