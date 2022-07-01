using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectBarrier : MonoBehaviour
{
    static int generatedCount = 0;
    private int stageNum{get; set;}  //���g�̃X�e�[�W�i���o�[(0�`) �������Ɉʒu���肷��
    private float timerPosY = 0;
    private float swingWidth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        stageNum = generatedCount;
        generatedCount++;
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�}�[�X�V
        UpdateTimer();

        //�ʒu�X�V
        SetPosAutoCalc();
    }

    void UpdateTimer()
    {
        timerPosY += Time.deltaTime;

        //���[�v
        if (timerPosY >= 2.0f)
        {
            timerPosY -= 2.0f;
        }
    }

    void SetPosAutoCalc()
    {
        //��]��
        if (StageSelectManager.GetInstance().IsStartMoveTimer())
        {
            float radius = StageSelectManager.GetInstance().GetRadius();

            float timer = StageSelectManager.GetInstance().GetMoveTimer();

            //��]�O
            int beforeNum = StageSelectManager.GetInstance().IsMoveLeft() ?
                StageSelectManager.GetNowSelectStageNum(false) - 1 + stageNum : StageSelectManager.GetNowSelectStageNum(false) + 1 + stageNum;
            float beforeRad = (360.0f / generatedCount * beforeNum - 90) * Mathf.Deg2Rad; //�J�����̑O�Ɏ����Ă��邽��270���̈ʒu��0�������ɂ���

            //��]��
            int afterNum = StageSelectManager.GetNowSelectStageNum(false) + stageNum;
            float afterRad = (360.0f / generatedCount * afterNum - 90) * Mathf.Deg2Rad; //�J�����̑O�Ɏ����Ă��邽��270���̈ʒu��0�������ɂ���

            transform.position = new Vector3(
                Mathf.Cos(EaseOutExpo(beforeRad, afterRad, timer / 0.75f)) * radius,
                Mathf.Sin(timerPosY * Mathf.PI) * swingWidth / 2,
                Mathf.Sin(EaseOutExpo(beforeRad, afterRad, timer / 0.75f)) * radius
                );

            transform.position += StageSelectManager.GetInstance().GetCenterPos();

        }
        //�Î~��
        else
        {
            transform.position = GetPosFromStageNum(stageNum);
        }

        //�o���A���̂���]�����鏈��
        if (StageSelectManager.GetInstance().IsStartDecideTimer() && StageSelectManager.GetNowSelectStageNum(true) == stageNum)
        {
            float timer = StageSelectManager.GetInstance().GetDecideTimer();
            transform.Rotate(0, EaseOutCubic(10,0, timer / 2.5f), 0);
        }
    }

    Vector3 GetPosFromStageNum(int stageNum)
    {
        Vector3 result;

        //�~��ɕ��ׂ�
        float dig = 360.0f / StageSelectManager.GetInstance().GetStageCount() * (stageNum - StageSelectManager.GetNowSelectStageNum(false));
        dig -= 90;  //�J�����̑O�Ɏ����Ă��邽��270���̈ʒu��0�������ɂ���

        float radius = StageSelectManager.GetInstance().GetRadius();

        result = new Vector3(Mathf.Cos(dig * Mathf.Deg2Rad) * radius, Mathf.Sin(timerPosY * Mathf.PI) * swingWidth / 2, Mathf.Sin(dig * Mathf.Deg2Rad) * radius);
        result += StageSelectManager.GetInstance().GetCenterPos();

        return result;
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

    float EaseOutCubic(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = 1 - Mathf.Pow(1 - t, 3);
        float a = e - s;
        v = s + a * v;

        return v;
    }
}
