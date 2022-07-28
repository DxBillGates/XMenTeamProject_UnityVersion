using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �K�E�Z�̃N���X
[System.Serializable]
public class UltimateSkill
{
    [SerializeField] private float maxGaugeValue;
    private int maxLevel;
    [SerializeField] private float initValue;

    // ���X�g�ɂ��ă��x���̏オ��₷���𒲐��ł���悤�ɂ��邩��
    [SerializeField] private List<float> levelUpValues;
    // ���x���ʂ̔�������
    [SerializeField] private List<float> levelsActiveTimes;
    // ���x���ʂ̑傫��
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
        // ���݂̃Q�[�W�ʂ����x���A�b�v�ɕK�v�Ȋ���l�Ŋ������l
        // ��j74(���Q�[�W��) / 25(����l) = 2level
        level = (int)(gaugeValue / (maxGaugeValue / maxLevel));
    }

    public void AddValue()
    {
        gaugeValue += levelUpValues[GetCurrentLevel()];

        // �Q�[�W�l�̐���
        if (gaugeValue >= maxGaugeValue) gaugeValue = maxGaugeValue;
    }

    // �X�L�����g�p
    public void Use(Vector3 position)
    {
        UltimateSkillGenerator.GetInstance().CreateSkillObject(position, levelsScales[level-1],CameraMotionManager.GetInstance().GetAnimationMaxTime());
    }

    // �������I��������
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

    // ���݂̃��x���ɐݒ肳��Ă���K�E�Z�̎��Ԃ�Ԃ�
    public float GetCurrentLevelActiveTime()
    {
        return levelsActiveTimes[level - 1];
    }

    public float GetCurrentLevelSize()
    {
        return levelsScales[level - 1];
    }
}
