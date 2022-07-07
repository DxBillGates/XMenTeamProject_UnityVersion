using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �X�L���g�p����|�[�Y���̃Q�[�������Ԃ̕ύX�Ȃǃv���C�V�[���݂̂œ��삳�������N���X
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
