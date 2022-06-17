using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    //�X�v���C�g
    [SerializeField] RectTransform circle;
    [SerializeField] RectTransform square;
    //�J�ڐ�̃V�[����
    [SerializeField] string nextSceneName;

    //���o�^�C�}�[
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
        //�^�C�}�[�Ɛ؂�ւ����o�X�V
        //�^�C�}�[�X�V
        timer += Time.deltaTime;

        //�ۂ�����
        circle.transform.position = new Vector3(
            EaseOutExpo(Screen.width + circle.sizeDelta.x / 2, -circle.sizeDelta.x / 2, timer / 0.5f),
            Screen.height / 2,
            0
            );

        //�l�p������
        square.transform.position = new Vector3(
            EaseOutExpo(Screen.width + square.sizeDelta.x / 2 + circle.sizeDelta.x / 2, Screen.width / 2, timer / 0.5f),
            Screen.height / 2,
            0
            );

        //�l�p�����Ɋg��
        blackSquareScale.x = EaseOutExpo(1, maxScaleX, timer / 0.5f);

        //�l�p���c�Ɋg��
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
