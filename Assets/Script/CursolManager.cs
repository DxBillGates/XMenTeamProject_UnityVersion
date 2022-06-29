using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursolManager : MonoBehaviour
{

    //�V�[���`�F���W�I�u�W�F�N�g
    [SerializeField] NextScene sceneChange;
    [SerializeField] Image title;
    [SerializeField] Image restart;
    [SerializeField] Image circle;
    [SerializeField] Image underLine;

    private int count;

    private int cursolPos = 0;  //0...Title 1...Restart

    private float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ++count;
        if(count>150&&Input.GetButtonDown("PlayerAbility"))
        {
            if (cursolPos == 0)
            {
                sceneChange.nextSceneName = "TitleScene";
            }
            else
            {
                sceneChange.nextSceneName = "Stage" + (StageSelectManager.GetNowSelectStageNum(true) + 1);
            }
            sceneChange.gameObject.SetActive(true);
        }

        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            timer = 0.5f;
        }

        //���E�L�[�������ꂽ�Ƃ�
        if (Input.GetAxis("Horizontal") < 0 && cursolPos > 0)
        {
            cursolPos--;
            timer = 0;
        }
        if (Input.GetAxis("Horizontal") > 0 && cursolPos < 1)
        {
            cursolPos++;
            timer = 0;
        }

        if (cursolPos == 0)
        {
            //Circle�ʒu
            circle.transform.position = new Vector3(210, 200);

            //UnderLine�ʒu�E�X�P�[��
            underLine.transform.position = new Vector3(
                EaseOutExpo(283, 283 + 132 * 1.85f / 2, timer / 0.5f),
                135
                );
            underLine.transform.localScale = new Vector3(
                EaseOutExpo(0, 1.85f, timer / 0.5f),
                2
                );

            //�F�ύX
            //�I�����W
            title.color = new Color32(242, 145, 25, 255);
            //��
            restart.color = new Color32(32, 174, 227, 255);
        }
        else
        {
            //Circle�ʒu
            circle.transform.position = new Vector3(700, 200);

            //UnderLine�ʒu�E�X�P�[��
            underLine.transform.position = new Vector3(
                EaseOutExpo(775, 775 + 132 * 2.8f / 2, timer / 0.5f),
                135
                );
            underLine.transform.localScale = new Vector3(
                EaseOutExpo(0, 2.8f, timer / 0.5f),
                2
                );


            //�F�ύX
            //��
            title.color = new Color32(32, 174, 227, 255);
            //�I�����W
            restart.color = new Color32(242, 145, 25, 255);
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
