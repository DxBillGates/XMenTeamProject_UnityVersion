using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : SingletonComponent<GameTimeManager>
{
    // �Q�[�������� 0 ~ 1
    [SerializeField, Range(0, 1)] private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 1;
    }

    void Update()
    {
        UltimateSkillManager skillManager = UltimateSkillManager.GetInstance();
        time = 1;
        if(skillManager.GetActiveFlagController().activeType == FlagActiveType.PRE)
        {
            time = 0.2f;
        }
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        // 0 ~ 1�𒴂����艺���Ȃ��悤�ɐ��`���
        value = Mathf.Lerp(0,1,value);
        time = value;
    }
}
