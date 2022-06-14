using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] RectTransform blackCircle;
    [SerializeField] RectTransform blackSquare;

    float timer = 0;
    bool isStart = false;
    Vector3 blackSquareScale = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        blackCircle.transform.position = new Vector3(Screen.width + blackCircle.sizeDelta.x / 2, Screen.height / 2, 0);
        blackSquare.transform.position = new Vector3(Screen.width + blackSquare.sizeDelta.x / 2 + blackCircle.sizeDelta.x / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Space�L�[�ŊJ�n
        if (Input.GetKey(KeyCode.Space))
        {
            isStart = true;
        }

        //�^�C�}�[�Ɛ؂�ւ����o�X�V
        if (isStart)
        {
            //�^�C�}�[�X�V
            timer += Time.deltaTime;

            //�ۂ�����
            blackCircle.transform.position = new Vector3(
                EaseOutExpo(Screen.width + blackCircle.sizeDelta.x / 2, -blackCircle.sizeDelta.x / 2, timer / 0.5f),
                Screen.height / 2,
                0
                );

            //�l�p������
            blackSquare.transform.position = new Vector3(
                EaseOutExpo(Screen.width + blackSquare.sizeDelta.x / 2 + blackCircle.sizeDelta.x / 2, Screen.width / 2, timer / 0.5f),
                Screen.height / 2,
                0
                );

            //�l�p�����Ɋg��
            blackSquareScale.x = EaseOutExpo(1, 8, timer / 0.5f);

            //�l�p���c�Ɋg��
            if (timer >= 0.75f)
            {
                blackSquareScale.y = EaseOutExpo(1, 15, (timer - 0.75f) / 0.5f);
            }

            blackSquare.transform.localScale = blackSquareScale;
        }

        if (timer >= 2)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    float EaseOutExpo(float s, float e, float t)
    {
        float v = t == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * t);
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
