using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkill : MonoBehaviour
{
    // �Q�[�W�̍ő�l
    [SerializeField] private float maxGaugeValue;
    [SerializeField] private int maxLevel;
    [SerializeField] private float initValue;

    // ���X�g�ɂ��ă��x���̏オ��₷���𒲐��ł���悤�ɂ��邩��
    [SerializeField] private float levelUpValue;

    // �g�p���Ă��邩
    private bool isUse;
    // �Q�[�W�̒l
    private float gaugeValue;
    // �Q�[�W�̃��x��
    private int level;
    // �g�p����
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


    // �X�L���̒l�𑝂₷
    public void AddValue(float addValue)
    {
        gaugeValue += addValue;

        // �Q�[�W�l�̐���
        if (gaugeValue >= maxGaugeValue) gaugeValue = maxGaugeValue;

        //// ���x�����v�Z
        //CalcLevel();
    }

    private void CalcLevel()
    {
        // ���݂̃Q�[�W�ʂ����x���A�b�v�ɕK�v�Ȋ���l�Ŋ������l
        // ��j74(���Q�[�W��) / 25(����l) = 2level
        level = (int)(gaugeValue / levelUpValue);
    }

    // ���x�����Ƃ̎g�p���Ԃ��v�Z����
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
        // ���x�����������Ă��Ȃ�������g�p���Ȃ�Ďg�p�͕s��
        if (level <= 0 || isUse) return;

        // ���x�����̎g�p���Ԃ��v�Z
        CalcLevelByUsingTime();


        // �Q�[�W�����������ă��x�����Čv�Z
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
