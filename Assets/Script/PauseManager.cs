using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PauseButtonType
{
    NONE,
    TITLE,
    RESTART,
    STAGE_SELECT,
    BACK,
}


public class PauseManager : MonoBehaviour
{
    private bool isPause;

    [SerializeField] private List<PauseButtonType> buttonInfos;
    private int currentButtonIndex;
    private bool isInputVerticalButton;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        isInputVerticalButton = false;
        currentButtonIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseButton();

        if (isPause == false) return;

        UpdateCurrentButtonIndex();

        switch (CheckUIButtons())
        {
            case PauseButtonType.TITLE:
                OnClickTitle();
                break;
            case PauseButtonType.RESTART:
                OnClickRestart();
                break;
            case PauseButtonType.STAGE_SELECT:
                OnClickStageSelect();
                break;
            case PauseButtonType.BACK:
                OnClickBack();
                break;
        }
    }

    private void CheckPauseButton()
    {
        if(Input.GetButtonDown("Pause"))
        {
            isPause = !isPause;
            //GameTimeManager.GetInstance().SetTime(System.Convert.ToInt32(isPause));
        }
    }

    // �|�[�Y���ɂǂ�UI�{�^���������ꂽ����Ԃ�
    private PauseButtonType CheckUIButtons()
    {
        if (isPause == false) return PauseButtonType.NONE;

        if(Input.GetButtonDown("PlayerAbility"))
        {
            return buttonInfos[currentButtonIndex];
        }

        return PauseButtonType.NONE;
    }

    // ���ݑI������Ă���UI�̎�ނ�Ԃ�
    private PauseButtonType GetCurrentSelectButton()
    {
        return buttonInfos[currentButtonIndex];
    }

    private void UpdateCurrentButtonIndex()
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        if (inputVertical != 0 && isInputVerticalButton == false)
        {
            currentButtonIndex += inputVertical > 0 ? 1 : -1;

            if (currentButtonIndex < 0) currentButtonIndex = 0;
            if (currentButtonIndex > buttonInfos.Count - 1) currentButtonIndex = buttonInfos.Count - 1;

            isInputVerticalButton = true;
        }

        if (inputVertical == 0) isInputVerticalButton = false;
    }

    // �^�C�g���ւ�UI���������ۂɎ��s������e
    private void OnClickTitle()
    {

    }

    // ���X�^�[�g��UI���������ۂɎ��s������e
    private void OnClickRestart()
    {

    }

    // �X�e�[�W�Z���N�g��UI���������ۂɎ��s������e
    private void OnClickStageSelect()
    {

    }

    // �߂��UI���������ۂɎ��s������e
    private void OnClickBack()
    {
        isPause = !isPause;
    }
}
