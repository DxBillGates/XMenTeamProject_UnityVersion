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

    // ポーズ中にどのUIボタンがおされたかを返す
    private PauseButtonType CheckUIButtons()
    {
        if (isPause == false) return PauseButtonType.NONE;

        if(Input.GetButtonDown("PlayerAbility"))
        {
            return buttonInfos[currentButtonIndex];
        }

        return PauseButtonType.NONE;
    }

    // 現在選択されているUIの種類を返す
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

    // タイトルへのUIを押した際に実行する内容
    private void OnClickTitle()
    {

    }

    // リスタートのUIを押した際に実行する内容
    private void OnClickRestart()
    {

    }

    // ステージセレクトのUIを押した際に実行する内容
    private void OnClickStageSelect()
    {

    }

    // 戻るのUIを押した際に実行する内容
    private void OnClickBack()
    {
        isPause = !isPause;
    }
}
