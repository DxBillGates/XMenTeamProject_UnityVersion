using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : MonoBehaviour
{
    // ゲージの最大値
    [SerializeField] private float maxGaugeValue;
    [SerializeField] private int maxLevel;
    [SerializeField] private float initValue;

    // リストにしてレベルの上がりやすさを調整できるようにするかも
    [SerializeField] private float levelUpValue;

    // 使用しているか
    private bool isUse;
    // ゲージの値
    private float gaugeValue;
    // ゲージのレベル
    private int level;
    // 使用時間
    private float usingTime;
    private float maxUsingTime;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        gaugeValue = initValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUse) return;
        if(usingTime >= maxUsingTime)
        {
            Initialize();
        }

        usingTime += Time.deltaTime;
    }

    private void Initialize()
    {
        isUse = false;
        gaugeValue = 0;
        level = 0;
    }


    // スキルの値を増やす
    public void AddValue(float addValue)
    {
        gaugeValue += addValue;

        // ゲージ値の制限
        if (gaugeValue >= maxGaugeValue) gaugeValue = maxGaugeValue;

        //// レベルを計算
        //CalcLevel();
    }

    private void CalcLevel()
    {
        // 現在のゲージ量をレベルアップに必要な既定値で割った値
        // 例）74(現ゲージ量) / 25(既定値) = 2level
        level = (int)(gaugeValue / levelUpValue);
    }

    // レベルごとの使用時間を計算する
    private void CalcLevelByUsingTime()
    {
        //1 * 1 = 1;
        //2 * 2 = 4;
        //3 * 3 = 9;
        //4 * 4 = 16;
        maxUsingTime = level * level;
    }

    public void Use()
    {
        // レベルが満たしていなかったり使用中なら再使用は不可
        if (level <= 0 || isUse) return;

        // レベル毎の使用時間を計算
        CalcLevelByUsingTime();


        // ゲージを初期化してレベルを再計算
        gaugeValue = 0;
        CalcLevel();

        isUse = true;
    }

    public int GetLevel()
    {
        CalcLevel();
        return level;
    }
}
