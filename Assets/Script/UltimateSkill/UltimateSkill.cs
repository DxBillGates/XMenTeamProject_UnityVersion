using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 必殺技のクラス
[System.Serializable]
public class UltimateSkill
{
    [SerializeField] private float maxGaugeValue;
    private int maxLevel;
    [SerializeField] private float initValue;

    // リストにしてレベルの上がりやすさを調整できるようにするかも
    [SerializeField] private List<float> levelUpValues;
    // レベル別の発動時間
    [SerializeField] private List<float> levelsActiveTimes;
    // レベル別の大きさ
    [SerializeField] private List<float> levelsScales;


    [SerializeField]private float gaugeValue;
    private int level;

    public void Initialize()
    {
        maxLevel = levelsActiveTimes.Count;
        gaugeValue = initValue;
    }

    public void Update()
    {
        CalcLevel();
    }

    private void CalcLevel()
    {
        // 現在のゲージ量をレベルアップに必要な既定値で割った値
        // 例）74(現ゲージ量) / 25(既定値) = 2level
        level = (int)(gaugeValue / (maxGaugeValue / maxLevel));
    }

    public void AddValue()
    {
        gaugeValue += levelUpValues[GetCurrentLevel()];

        // ゲージ値の制限
        if (gaugeValue >= maxGaugeValue) gaugeValue = maxGaugeValue;
    }

    // スキルを使用
    public void Use(Vector3 position)
    {
        UltimateSkillGenerator.GetInstance().CreateSkillObject(position, levelsScales[level-1],CameraMotionManager.GetInstance().GetAnimationMaxTime());
    }

    // 発動を終了させる
    public void End()
    {
        UltimateSkillGenerator.GetInstance().DestroySkillObject();
        gaugeValue = 0;
        level = 0;
    }

    public int GetCurrentLevel()
    {
        return level;
    }

    // 現在のレベルに設定されている必殺技の時間を返す
    public float GetCurrentLevelActiveTime()
    {
        return levelsActiveTimes[level - 1];
    }

    public float GetCurrentLevelSize()
    {
        return levelsScales[level - 1];
    }
}
