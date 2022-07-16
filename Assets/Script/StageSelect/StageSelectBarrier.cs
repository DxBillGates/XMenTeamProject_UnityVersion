using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectBarrier : MonoBehaviour
{
    private int stageNum = -1;  //自身のステージナンバー(0～) これを基に位置決定する
    private float timerPosY = 0;
    private float swingWidth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        timerPosY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー更新
        UpdateTimer();

        //位置更新
        SetPosAutoCalc();
    }

    void UpdateTimer()
    {
        timerPosY += Time.deltaTime;

        //ループ
        if (timerPosY >= 2.0f)
        {
            timerPosY -= 2.0f;
        }
    }

    void SetPosAutoCalc()
    {
        //回転時
        if (StageSelectManager.GetInstance().IsStartMoveTimer())
        {
            //半径
            float radius = StageSelectManager.GetInstance().GetRadius();

            //移動タイマー
            float timer = StageSelectManager.GetInstance().GetMoveTimer();

            //移動タイマーの上限値
            float limitTimer = StageSelectManager.GetInstance().GetLimitMoveTimer();

            //ステージ数
            int stageCount = StageSelectManager.GetInstance().GetStageCount();

            //回転前
            int beforeNum = StageSelectManager.GetInstance().IsMoveLeft() ?
                StageSelectManager.GetNowSelectStageNum(false) - 1 + stageNum : StageSelectManager.GetNowSelectStageNum(false) + 1 + stageNum;
            float beforeRad = (360.0f / stageCount * beforeNum - 90) * Mathf.Deg2Rad; //カメラの前に持ってくるため270°の位置を0°扱いにする

            //回転後
            int afterNum = StageSelectManager.GetNowSelectStageNum(false) + stageNum;
            float afterRad = (360.0f / stageCount * afterNum - 90) * Mathf.Deg2Rad; //カメラの前に持ってくるため270°の位置を0°扱いにする

            //位置
            transform.position = new Vector3(
                Mathf.Cos(EaseOutExpo(beforeRad, afterRad, timer / limitTimer)) * radius,
                Mathf.Sin(timerPosY * Mathf.PI) * swingWidth / 2,
                Mathf.Sin(EaseOutExpo(beforeRad, afterRad, timer / limitTimer)) * radius
                );

            transform.position += StageSelectManager.GetInstance().GetCenterPos();

            //回転
            transform.rotation = Quaternion.Euler(
                0,
                EaseOutExpo(90 - (beforeRad * Mathf.Rad2Deg), 90 - (afterRad * Mathf.Rad2Deg), timer / limitTimer),
                0);

        }
        //静止時
        else
        {
            transform.position = GetPosFromStageNum(stageNum);
            transform.rotation = GetRotationFromStageNum(stageNum);
        }

        //バリア自体を回転させる処理
        if (StageSelectManager.GetInstance().IsStartDecideTimer() && StageSelectManager.GetNowSelectStageNum(true) == stageNum)
        {
            float timer = StageSelectManager.GetInstance().GetDecideTimer();
            float limitTimer = StageSelectManager.GetInstance().GetLimitDecideTimer();
            transform.rotation = Quaternion.Euler(0, EaseOutCubic(180 + 360 * StageSelectManager.GetInstance().GetRotationCount(), 180, timer / (limitTimer - 1.0f)), 0);
        }
    }

    Vector3 GetPosFromStageNum(int stageNum)
    {
        Vector3 result;

        //円状に並べる
        float dig = 360.0f / StageSelectManager.GetInstance().GetStageCount() * (stageNum - StageSelectManager.GetNowSelectStageNum(false));
        dig -= 90;  //カメラの前に持ってくるため270°の位置を0°扱いにする

        float radius = StageSelectManager.GetInstance().GetRadius();

        result = new Vector3(Mathf.Cos(dig * Mathf.Deg2Rad) * radius, Mathf.Sin(timerPosY * Mathf.PI) * swingWidth / 2, Mathf.Sin(dig * Mathf.Deg2Rad) * radius);
        result += StageSelectManager.GetInstance().GetCenterPos();

        return result;
    }

    Quaternion GetRotationFromStageNum(int stageNum)
    {
        //円状に並べる
        float dig = 360.0f / StageSelectManager.GetInstance().GetStageCount() * (stageNum - StageSelectManager.GetNowSelectStageNum(false));

        return Quaternion.Euler(0, 180 - dig, 0);
    }

    public void SetStageNum(int num)
    {
        stageNum = num;
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
