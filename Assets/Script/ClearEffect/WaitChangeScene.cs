using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitChangeScene : MonoBehaviour
{
    // �ҋ@���Ԃ����ȏゾ�ƃV�[����ς���
    [SerializeField] private float waitTimer = 10;

    [SerializeField] private NextScene nextScene;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // �o�ߎ��Ԍv���p
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // �o�ߎ��ԗp
        float nowTime = Time.time;

        if(Input.anyKey)
        {
            startTime = Time.time;
        }

        // �w�莞�ԉ������삳��ĂȂ�������
        if(nowTime - startTime > waitTimer)
        {
            nextScene.nextSceneName = "TitleScene";
            nextScene.gameObject.SetActive(true);
        }
    }
}
