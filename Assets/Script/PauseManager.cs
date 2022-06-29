using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PauseButtonType
{
    NONE,
    BGM_VOLUME,
    SE_VOLUME,
    TITLE,
    RESTART,
    STAGE_SELECT,
    BACK,
}


public class PauseManager : MonoBehaviour
{
    private bool isPause;

    [SerializeField] private List<PauseButtonType> buttonInfos;
    [SerializeField] private int currentButtonIndex;
    private bool isInputVerticalButton;
    private bool isInputHorizontalButton;

    [SerializeField] private UnityEngine.UI.Text testTextUI;
    [SerializeField] private List<UnityEngine.UI.Text> texts;

    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        isInputVerticalButton = isInputHorizontalButton = false;
        currentButtonIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseButton();
        testTextUI.gameObject.SetActive(isPause);

        if (isPause == false) return;

        UpdateCurrentButtonIndex();
        UpdateUIColor();
        UpdateAudioVolumeUI();

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
        if (Input.GetButtonDown("Pause"))
        {
            isPause = !isPause;

            // trueだと1になるから!をつける
            GameTimeManager.GetInstance().SetTime(System.Convert.ToInt32(!isPause));
        }
    }

    // ポーズ中にどのUIボタンがおされたかを返す
    private PauseButtonType CheckUIButtons()
    {
        if (isPause == false) return PauseButtonType.NONE;

        if (Input.GetButtonDown("PlayerAbility"))
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
        if (Input.GetAxisRaw("VerticalButton") != 0) inputVertical = Input.GetAxisRaw("VerticalButton");

        if (inputVertical != 0 && isInputVerticalButton == false)
        {
            currentButtonIndex += inputVertical > 0 ? -1 : 1;

            if (currentButtonIndex < 0) currentButtonIndex = 0;
            if (currentButtonIndex > buttonInfos.Count - 1) currentButtonIndex = buttonInfos.Count - 1;

            isInputVerticalButton = true;
        }

        if (inputVertical == 0) isInputVerticalButton = false;
    }

    private void UpdateUIColor()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            if (i == currentButtonIndex)
            {
                texts[i].color = Color.red;
            }
            else
            {
                texts[i].color = Color.gray;
            }
        }
    }

    private void UpdateAudioVolumeUI()
    {
        if (isPause == false) return;
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("HorizontalButton") != 0) inputHorizontal = Input.GetAxisRaw("HorizontalButton");

        if(inputHorizontal != 0 && isInputHorizontalButton == false)
        {
            int increaseValue = inputHorizontal > 0 ? 1 : -1;
            if(currentButtonIndex == (int)PauseButtonType.BGM_VOLUME - 1)
            {
                AudioManager.GetInstance().IncreaseBGMMasterVolumeLevel(increaseValue);
            }
            if(currentButtonIndex == (int)PauseButtonType.SE_VOLUME - 1)
            {
                AudioManager.GetInstance().IncreaseSEMasterVolumeLevel(increaseValue);
            }

            isInputHorizontalButton = true;
        }

        if (inputHorizontal == 0) isInputHorizontalButton = false;

        texts[(int)PauseButtonType.BGM_VOLUME - 1].text = "BGMVolume" + " < " + AudioManager.GetInstance().GetBGMMasterVolumeLevel() + " > ";
        texts[(int)PauseButtonType.SE_VOLUME - 1].text = "SEVolume" + " < " + AudioManager.GetInstance().GetSEMasterVolumeLevel() + " > ";
    }

    // タイトルへのUIを押した際に実行する内容
    private void OnClickTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }

    // リスタートのUIを押した際に実行する内容
    private void OnClickRestart()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }

    // ステージセレクトのUIを押した際に実行する内容
    private void OnClickStageSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
    }

    // 戻るのUIを押した際に実行する内容
    private void OnClickBack()
    {
        isPause = !isPause;
        // trueだと1になるから!をつける
        GameTimeManager.GetInstance().SetTime(System.Convert.ToInt32(!isPause));
    }
}
