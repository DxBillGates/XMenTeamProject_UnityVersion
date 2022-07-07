using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : SingletonComponent<GameTimeManager>
{
    // ÉQÅ[ÉÄì‡éûä‘ 0 ~ 1
    [SerializeField, Range(0, 1)] private float time = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        CheckUltimateManager();
        CheckPauseManager();
    }

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float value)
    {
        // 0 ~ 1Çí¥Ç¶ÇΩÇËâ∫âÒÇÁÇ»Ç¢ÇÊÇ§Ç…ê¸å`ï‚ä‘
        value = Mathf.Lerp(0, 1, value);
        time = value;
    }

    private void CheckUltimateManager()
    {
        UltimateSkillManager ultManager = UltimateSkillManager.GetInstance();
        FlagController activeFlagController = ultManager.GetActiveFlagController();

        if (ultManager.IsUse() == false) return;

        if(activeFlagController.activeType == FlagActiveType.PRE)
        {
            const float SLOW_TIME = 0.05f;
            SetTime(SLOW_TIME);
        }
        else
        {
            SetTime(1);
        }
    }

    private void CheckPauseManager()
    {
        PauseManager pauseManager = PauseManager.GetInstance();

        if (pauseManager.IsPause() == false) return;

        SetTime(0);
    }
}