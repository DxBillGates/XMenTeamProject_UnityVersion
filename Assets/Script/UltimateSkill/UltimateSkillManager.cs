using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �K�E�Z�̔����Ȃǂ��Ǘ�����N���X
public class UltimateSkillManager : SingletonComponent<UltimateSkillManager>
{
    [SerializeField] private FlagController preActiveFlagController;
    [SerializeField] private FlagController activeFlagController;
    [SerializeField] private FlagController endActiveFlagController;
    [SerializeField] private UltimateSkill ultimateSkill;

    // �g�p�����ǂ���
    private bool isUse;

    // �g�p���n�߂��ꏊ
    public Vector3 usedPosition { get; private set; }

    // �g�p�����ۂ̃T�C�Y
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
        //// test
        //if (isUse == false && Input.GetKeyDown(KeyCode.Space))
        //{
        //}
        ultimateSkill.Update();

        if (isUse == false) return;

        // �X�L�������O�̃t���O�X�V����
        preActiveFlagController.Update(Time.deltaTime);
        if (preActiveFlagController.IsEndTrigger() == true)
        {
            ultimateSkill.Use(usedPosition);
            activeFlagController.maxActiveTime = ultimateSkill.GetCurrentLevelActiveTime();
            activeFlagController.flag = true;
        }

        // �X�L���������̃t���O�X�V����
        activeFlagController.Update(Time.deltaTime);
        if (activeFlagController.IsEndTrigger() == true)
        {
            ultimateSkill.End();
            endActiveFlagController.flag = true;
        }


        // �X�L��������̃t���O��������
        endActiveFlagController.Update(Time.deltaTime);
        if (endActiveFlagController.IsEndTrigger() == true) Initialize();
    }

    // �g�p�ꏊ���w�肵�ăX�L���𔭓�����
    public void Use(Vector3 position = new Vector3())
    {
        // �K�E�Z���x����0�ȉ��̏ꍇ�͔��������Ȃ�
        if (ultimateSkill.GetCurrentLevel() <= 0) return;

        usedPosition = position;
        usedSize = ultimateSkill.GetCurrentLevelSize();
        isUse = true;
        preActiveFlagController.flag = true;
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
        // pre �� �{�����m�F���ăt���O���I���Ȃ炻��FlagController��Ԃ�
        if (preActiveFlagController.flag == true) return preActiveFlagController;
        if (activeFlagController.flag == true) return activeFlagController;

        // ����ȊO�Ȃ�I������flagController��Ԃ�
        return endActiveFlagController;
    }

    // �A�N�e�B�u�̃t���O�R���g���[���[�����쒆����Ԃ�
    public bool IsUse()
    {
        return activeFlagController.flag;
    }

    public int GetCurrentLevel()
    {
        return ultimateSkill.GetCurrentLevel();
    }
}
