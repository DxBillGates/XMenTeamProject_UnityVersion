using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : SingletonComponent<StageSelectManager>
{
    [SerializeField] int stageCount = 5;
    [SerializeField] Vector3 center = new Vector3(0, 0, 20);    //回転の中止座標
    [SerializeField] float radius = 20;                         //回転する円の半径
    [SerializeField] GameObject barrierPrefab;                  //バリアプレハブ
    [SerializeField] List<Image> UIStageNums;                   //選択中のステージナンバーUI
    [SerializeField] List<Sprite> sprNums;                      //0～9の数字
    [SerializeField] GameObject sceneChange;                    //シーンチェンジオブジェクト

    static int staticStageCount;                        //ステージ数　いずれ自動カウントできるようにしたい
    static int nowSelectStageNum = 0;                                  //現在選択中のステージインデックス
    [SerializeField] float timer = 0.5f;                        //演出タイマー
    bool isStartTimer = false;                                  //タイマーが開始しているか
    bool isMoveLeft = false;                                    //バリアが左に動いているか


    protected override void Awake()
    {
        staticStageCount = stageCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stageCount; i++)
        {
            Instantiate(barrierPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //左右キーが押されたとき、回転させる
        if (isStartTimer)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                nowSelectStageNum--;
                timer = 0;
                isStartTimer = true;
                isMoveLeft = false;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                nowSelectStageNum++;
                timer = 0;
                isStartTimer = true;
                isMoveLeft = true;

            }
        }
        if (timer > 0.5f)
        {
            timer = 0.5f;
            isStartTimer = false;
        }

        SetSpriteNum(GetNowSelectStageNum(true) + 1);

        //決定
        if (Input.GetButtonDown("PlayerAbility"))
        {
            //壁の数を指定して作り直し
            sceneChange.SetActive(true);
        }
    }

    public int GetStageCount()
    {
        return stageCount;
    }

    static public int GetNowSelectStageNum(bool isNormalize)
    {
        int result = nowSelectStageNum;
        if (isNormalize)
        {
            while (result < 0)
            {
                result += staticStageCount;
            }

            return result % staticStageCount;
        }
        else
        {
            return nowSelectStageNum;
        }

    }

    public float GetRadius()
    {
        return radius;
    }

    public Vector3 GetCenterPos()
    {
        return center;
    }

    public float GetTimer()
    {
        return timer;
    }

    public bool IsStartTimer()
    {
        return isStartTimer;
    }

    public bool IsMoveLeft()
    {
        return isMoveLeft;
    }

    void SetSpriteNum(int num)
    {
        if (num > 99)
        {
            num = 99;
        }
        else if (num < 0)
        {
            num = 0;
        }

        int cal = num;
        int index = cal % 10;
        //一の位
        UIStageNums[0].sprite = sprNums[index];

        cal /= 10;
        index = cal % 10;
        //十の位
        UIStageNums[1].sprite = sprNums[index];
    }
}
