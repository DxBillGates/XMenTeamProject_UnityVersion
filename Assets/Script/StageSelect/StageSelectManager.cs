using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : SingletonComponent<StageSelectManager>
{
    [SerializeField] int stageCount = 3;
    [SerializeField] Vector3 center = new Vector3(0, 0, 20);    //回転の中止座標
    [SerializeField] float radius = 20;                         //回転する円の半径
    [SerializeField] GameObject barrierPrefab;                  //バリアプレハブ
    [SerializeField] List<Image> UIStageNums;                   //選択中のステージナンバーUI
    [SerializeField] List<Sprite> sprNums;                      //0～9の数字
    [SerializeField] NextScene sceneChange;                     //シーンチェンジオブジェクト
    [SerializeField] int rotationCount = 2;                     //ステージ決定時のオブジェクト回転数
    [SerializeField] float limitMoveTimer = 1.5f;               //移動タイマーの上限値
    [SerializeField] float limitDecideTimer = 2.5f;             //決定タイマーの上限値
    [SerializeField] List<Texture> textures;                  //盾にセットするマテリアル

    static int staticStageCount;                                //ステージ数　いずれ自動カウントできるようにしたい
    static int nowSelectStageNum = 0;                           //現在選択中のステージインデックス
    bool isMoveLeft = false;                                    //バリアが左に動いているか
    float moveTimer = 0.75f;                                    //移動タイマー
    bool isStartMoveTimer = false;                              //移動タイマーが開始しているか
    float decideTimer = 0;                                      //移動タイマー
    bool isStartDecideTimer = false;                            //移動タイマーが開始しているか

    [SerializeField] GameObject circleShadowScriptObject;       //丸影のスクリプト取得用

    protected override void Awake()
    {
        staticStageCount = stageCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stageCount; i++)
        {
            GameObject localScopeGameObject =  Instantiate(barrierPrefab);
            StageSelectBarrier barrier = localScopeGameObject.GetComponent<StageSelectBarrier>();
            barrier.SetStageNum(i);
            GameObject child = barrier.transform.Find("MeshSub").gameObject;
            child.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[i]);
            circleShadowScriptObject.GetComponent<CircleShadow>().AddObject(localScopeGameObject);
        }
        moveTimer = 0;
        decideTimer = 0;
        isStartDecideTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //タイマー更新
        UpdateTimer();

        //左右キーが押されたとき、回転させる
        if (isStartMoveTimer == false)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                nowSelectStageNum++;
                moveTimer = 0;
                isStartMoveTimer = true;
                isMoveLeft = false;
            } 
            if (Input.GetAxis("Horizontal") < 0)
            {
                nowSelectStageNum--;
                moveTimer = 0;
                isStartMoveTimer = true;
                isMoveLeft = true;
            }
        }

        //Stagennのスプライトセット
        SetSpriteNum(GetNowSelectStageNum(true) + 1);

        //決定
        if (Input.GetButtonDown("PlayerAbility"))
        {
            isStartDecideTimer = true;
        }
        //演出途中で次のシーンへ
        if (decideTimer >= 1.0f && sceneChange.gameObject.activeSelf == false)
        {
            //選ばれたステージシーンに遷移
            sceneChange.nextSceneName = "Stage" + (GetNowSelectStageNum(true) + 1).ToString();
            sceneChange.gameObject.SetActive(true);
        }

        //ポーズボタンでタイトルへ
        if (Input.GetButtonDown("Pause"))
        {
            sceneChange.nextSceneName = "TitleScene";
            sceneChange.gameObject.SetActive(true);
        }
    }

    void UpdateTimer()
    {
        if (isStartMoveTimer)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > limitMoveTimer)
            {
                moveTimer = limitMoveTimer;
                isStartMoveTimer = false;
            }
        }
        if (isStartDecideTimer)
        {
            decideTimer += Time.deltaTime;
            if (decideTimer > limitDecideTimer)
            {
                decideTimer = limitDecideTimer;
                isStartDecideTimer = false;
            }
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

            return staticStageCount != 0 ? result % staticStageCount : 0;
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

    public int GetRotationCount()
    {
        return rotationCount;
    }
    public bool IsMoveLeft()
    {
        return isMoveLeft;
    }
    public float GetMoveTimer()
    {
        return moveTimer;
    }

    public bool IsStartMoveTimer()
    {
        return isStartMoveTimer;
    }
    public float GetLimitMoveTimer()
    {
        return limitMoveTimer;
    }
    public float GetDecideTimer()
    {
        return decideTimer;
    }
    public bool IsStartDecideTimer()
    {
        return isStartDecideTimer;
    }
    public float GetLimitDecideTimer()
    {
        return limitDecideTimer;
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
