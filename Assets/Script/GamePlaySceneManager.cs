using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// スキル使用時やポーズ時のゲーム内時間の変更などプレイシーンのみで動作させたいクラス
public class GamePlaySceneManager : MonoBehaviour
{
    GameTimeManager gameTimeManager;
    // Start is called before the first frame update
    void Start()
    {
        gameTimeManager = GameTimeManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        CheckUltimateManager();
        CheckPauseManager();
    }

    private void CheckUltimateManager()
    {
        UltimateSkillManager ultManager = UltimateSkillManager.GetInstance();
        FlagController activeFlagController = ultManager.GetActiveFlagController();

        if (ultManager.IsUse() == false) return;

        if (activeFlagController.activeType == FlagActiveType.PRE)
        {
            const float SLOW_TIME = 0.05f;
            gameTimeManager.SetTime(SLOW_TIME);
        }
        else
        {
            gameTimeManager.SetTime(1);
        }
    }

    private void CheckPauseManager()
    {
        if (PauseManager.IsPause() == false) return;

        gameTimeManager.SetTime(0);
    }
}
