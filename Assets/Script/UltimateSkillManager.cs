using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkillManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject ultimateSkillGaugeObject;
    [SerializeField] private float addValue;

    private UltimateSkill ultimateSkill;
    private UltimateGauge ultimateSkillGauge;

    // Start is called before the first frame update
    void Start()
    {
        ultimateSkill = playerObject.GetComponent<UltimateSkill>();
        ultimateSkillGauge = ultimateSkillGaugeObject.GetComponent<UltimateGauge>();
    }

    // Update is called once per frame
    void Update()
    {
        ultimateSkillGauge.SetLevel(ultimateSkill.GetLevel());

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddGaugeValue();
        }
    }

    // �G��|�����Ƃ��ɌĂ�
    void AddGaugeValue()
    {
        ultimateSkill.AddValue(addValue);
    }
}
