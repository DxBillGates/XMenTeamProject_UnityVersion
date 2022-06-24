using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectBarrier : MonoBehaviour
{
    static int generatedCount = 0;
    private int stageNum{get; set;}  //ステージナンバー(0～) これを基に位置決定する

    // Start is called before the first frame update
    void Start()
    {
        stageNum = generatedCount;
        generatedCount++;
    }

    // Update is called once per frame
    void Update()
    {
        SetPosAutoCalc();
    }

    void SetPosAutoCalc()
    {
        //回転時
        if (StageSelectManager.GetInstance().IsStartTimer())
        {
            float timer = StageSelectManager.GetInstance().GetTimer();
            //回転前
            Vector3 before = StageSelectManager.GetInstance().IsMoveLeft() ?
                GetPosFromStageNum(stageNum + 1) : GetPosFromStageNum(stageNum - 1);

            //回転後
            Vector3 after = GetPosFromStageNum(stageNum);

            transform.position = new Vector3(
                Lerp(before.x, after.x, timer / 0.5f),
                Lerp(before.y, after.y, timer / 0.5f),
                Lerp(before.z, after.z, timer / 0.5f)
                );
        }
        //静止時
        else
        {
            transform.position = GetPosFromStageNum(stageNum);
        }
    }

    Vector3 GetPosFromStageNum(int stageNum)
    {
        Vector3 result = new Vector3();

        //円状に並べる
        float dig = 360.0f / StageSelectManager.GetInstance().GetStageCount() * (stageNum - StageSelectManager.GetNowSelectStageNum(false));
        dig -= 90;  //カメラの前に持ってくるため270°の位置を0°扱いにする

        float radius = StageSelectManager.GetInstance().GetRadius();

        result = new Vector3(Mathf.Cos(dig * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(dig * Mathf.Deg2Rad) * radius);
        result += StageSelectManager.GetInstance().GetCenterPos();

        return result;
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
}
