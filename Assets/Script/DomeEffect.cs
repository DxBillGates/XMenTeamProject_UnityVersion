using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeEffect : MonoBehaviour
{

    [SerializeField] Material material;

    float threshold;

    float glowIntencity;
    // Start is called before the first frame update
    void Start()
    {
        threshold = 0.999f;
        glowIntencity = 0.056f;
    }

    // Update is called once per frame
    void Update()
    {
        var ultManager = UltimateSkillManager.GetInstance();
        var activeFlagController = ultManager.GetActiveFlagController();
        float time = 1;
        if (activeFlagController.flag == true && activeFlagController.activeType == FlagActiveType.END)
        {
            time = activeFlagController.activeTime / activeFlagController.maxActiveTime;

            time = Easing.EaseInExpo(time);
            SetThreshold(1 - time);
        }

        material.SetFloat("_Threshold", threshold);
        material.SetFloat("_GlowCutoff", glowIntencity);
    }

    public void SetThreshold(float t)
    {
        threshold = t < 0 ? 0 : t > 0.999f ? 0.999f : t;
    }
    public void SetGlowIntencity(float i)
    {
        glowIntencity = i < 0 ? 0 : i > 1 ? 1 : i;
    }

}
