using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 必殺技の発動などを管理するクラス
public class UltimateSkillManager : SingletonComponent<UltimateSkillManager>
{
    [SerializeField] private FlagController preActiveFlagController;
    [SerializeField] private FlagController activeFlagController;
    [SerializeField] private FlagController endActiveFlagController;
    [SerializeField] private UltimateSkill ultimateSkill;

    // 使用中かどうか
    private bool isUse;

    // 使用し始めた場所
    public Vector3 usedPosition { get; private set; }

    // 使用した際のサイズ
    public float usedSize { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        preActiveFlagController.activeType = FlagActiveType.PRE;
        activeFlagController.activeType = FlagActiveType.ACTIVE;
        endActiveFlagController.activeType = FlagActiveType.END;

        Initialize();
        ultimateSkill.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (endActiveFlagController.IsEndTrigger() == true) Initialize();

        ultimateSkill.Update();

        if (isUse == false) return;

        // スキル発動前のフラグ更新処理
        preActiveFlagController.Update(Time.deltaTime);
        if (preActiveFlagController.IsEndTrigger() == true)
        {
            ultimateSkill.Use(usedPosition);
            activeFlagController.maxActiveTime = ultimateSkill.GetCurrentLevelActiveTime();
            activeFlagController.flag = true;
        }

        // スキル発動中のフラグ更新処理
        activeFlagController.Update(Time.deltaTime);
        if (activeFlagController.IsEndTrigger() == true)
        {
            ultimateSkill.End();
            endActiveFlagController.flag = true;
        }


        // スキル発動後のフラグ発動処理
        endActiveFlagController.Update(Time.deltaTime);
    }

    // 使用場所を指定してスキルを発動する
    public void Use(Vector3 position,Vector3 direction,Transform triggerTransform)
    {
        // 必殺技レベルが0以下の場合は発動させない
        if (ultimateSkill.GetCurrentLevel() <= 0) return;
        if (isUse == true) return;

        usedPosition = position;
        usedSize = ultimateSkill.GetCurrentLevelSize();
        isUse = true;
        preActiveFlagController.flag = true;
        preActiveFlagController.maxActiveTime = CameraMotionManager.GetInstance().GetFlagController().maxActiveTime;

        const float DIRECTION_VALUE = 10;
        CameraMotionManager.GetInstance().StartPreUltMotion(position + direction * DIRECTION_VALUE, triggerTransform);
    }

    private void Initialize()
    {
        preActiveFlagController.Initialize();

        activeFlagController = new FlagController();
        activeFlagController.Initialize();
        activeFlagController.activeType = FlagActiveType.ACTIVE;

        endActiveFlagController.Initialize();
        isUse = false;
    }

    public FlagController GetActiveFlagController()
    {
        // pre と 本命を確認してフラグがオンならそのFlagControllerを返す
        if (preActiveFlagController.flag == true) return preActiveFlagController;
        if (activeFlagController.flag == true) return activeFlagController;

        // それ以外なら終了時のflagControllerを返す
        return endActiveFlagController;
    }

    // アクティブのフラグコントローラーが動作中かを返す
    public bool IsUse()
    {
        return activeFlagController.flag;
    }

    public int GetCurrentLevel()
    {
        return ultimateSkill.GetCurrentLevel();
    }
}
