using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PauseButtonType
{
    NONE = -1,
    BGM_VOLUME,
    SE_VOLUME,
    TITLE,
    RESTART,
    STAGE_SELECT,
    BACK,
}


public class PauseManager : SingletonComponent<PauseManager>
{
    static bool isPause;

    [SerializeField] private List<PauseButtonType> buttonInfos;
    private int currentButtonIndex;
    private bool isInputVerticalButton;
    private bool isInputHorizontalButton;

    [SerializeField] private UnityEngine.UI.Text testTextUI;
    [SerializeField] private List<UnityEngine.UI.Text> texts;
    [SerializeField] private CanvasGroup BGAlpha;
    [SerializeField] private RectTransform circle;
    [SerializeField] private RectTransform underLine;
    [SerializeField] private Image bgmVolume;
    [SerializeField] private Image seVolume;
    [SerializeField] private Image bgmArrowLeft;
    [SerializeField] private Image bgmArrowRight;
    [SerializeField] private Image seArrowLeft;
    [SerializeField] private Image seArrowRight;
    [SerializeField] private List<Sprite> sprNum;

    // ポーズ中の操作が何秒後から可能か
    [SerializeField] private float enabledTime;

    // ポーズ中の経過時間
    private float pauseTime;

    // 球と下線の演出用タイマー
    private float timerEffect = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPauseButton();
        testTextUI.gameObject.SetActive(isPause);
        UpdateUIColor();

        if (isPause == false) return;

        UpdateAudioVolumeUI();
        UpdateEffect();

        pauseTime += Time.deltaTime;

        // ポーズ中の経過時間が操作可能時間に満たしていないなら操作はできないようにする
        if (pauseTime < enabledTime) return;

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

    private void Initialize()
    {
        isPause = false;
        isInputVerticalButton = isInputHorizontalButton = true;
        currentButtonIndex = 0;
        pauseTime = 0;
        timerEffect = 0;
    }

    private void CheckPauseButton()
    {
        if (Input.GetButtonDown("Pause"))
        {
            bool backupIsPause = isPause = !isPause;

            // trueだと1になるから!をつける
            if (StartGameScene.IsGameStart())
            {
                GameTimeManager.GetInstance().SetTime(System.Convert.ToInt32(!isPause));
            }

            Initialize();
            isPause = backupIsPause;
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
            timerEffect = 0;
        }

        if (inputVertical == 0) isInputVerticalButton = false;
    }

    // 選択されているUIの色を変更する
    private void UpdateUIColor()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            if (i == currentButtonIndex)
            {
                texts[i].color = new Color32(242, 145, 25, 255);
            }
            else
            {
                texts[i].color = new Color32(32, 174, 227, 255);
            }
        }
        //BGM数字
        if (currentButtonIndex == (int)PauseButtonType.BGM_VOLUME)
        {
            bgmVolume.color = new Color32(242, 145, 25, 255);
        }
        else
        {
            bgmVolume.color = new Color32(30, 173, 226, 255);
        }
        //SE数字
        if (currentButtonIndex == (int)PauseButtonType.SE_VOLUME)
        {
            seVolume.color = new Color32(242, 145, 25, 255);
        }
        else
        {
            seVolume.color = new Color32(30, 173, 226, 255);
        }
        //BGの色
        if (isPause == false)
        {
            BGAlpha.alpha = 0;
        }
        else
        {
            BGAlpha.alpha = 0.75f;
        }
    }

    private void UpdateAudioVolumeUI()
    {
        if (isPause == false) return;
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("HorizontalButton") != 0) inputHorizontal = Input.GetAxisRaw("HorizontalButton");

        if (inputHorizontal != 0 && isInputHorizontalButton == false)
        {
            int increaseValue = inputHorizontal > 0 ? 1 : -1;
            if (currentButtonIndex == (int)PauseButtonType.BGM_VOLUME)
            {
                AudioManager.GetInstance().IncreaseBGMMasterVolumeLevel(increaseValue);
            }
            if (currentButtonIndex == (int)PauseButtonType.SE_VOLUME)
            {
                AudioManager.GetInstance().IncreaseSEMasterVolumeLevel(increaseValue);
            }

            isInputHorizontalButton = true;
        }

        //スプライト更新
        //数字
        bgmVolume.sprite = sprNum[AudioManager.GetInstance().GetBGMMasterVolumeLevel()];
        seVolume.sprite = sprNum[AudioManager.GetInstance().GetSEMasterVolumeLevel()];
        //矢印
        //BGM
        if (currentButtonIndex == (int)PauseButtonType.BGM_VOLUME)
        {
            bgmArrowLeft.color = new Color(bgmArrowLeft.color.r, bgmArrowLeft.color.g, bgmArrowLeft.color.b, 1);
            bgmArrowRight.color = new Color(bgmArrowRight.color.r, bgmArrowRight.color.g, bgmArrowRight.color.b, 1);
        }
        else
        {
            bgmArrowLeft.color = new Color(bgmArrowLeft.color.r, bgmArrowLeft.color.g, bgmArrowLeft.color.b, 0);
            bgmArrowRight.color = new Color(bgmArrowRight.color.r, bgmArrowRight.color.g, bgmArrowRight.color.b, 0);
        }
        //SE
        if (currentButtonIndex == (int)PauseButtonType.SE_VOLUME)
        {
            seArrowLeft.color = new Color(seArrowLeft.color.r, seArrowLeft.color.g, seArrowLeft.color.b, 1);
            seArrowRight.color = new Color(seArrowRight.color.r, seArrowRight.color.g, seArrowRight.color.b, 1);
        }
        else
        {
            seArrowLeft.color = new Color(seArrowLeft.color.r, seArrowLeft.color.g, seArrowLeft.color.b, 0);
            seArrowRight.color = new Color(seArrowRight.color.r, seArrowRight.color.g, seArrowRight.color.b, 0);
        }

        if (inputHorizontal == 0) isInputHorizontalButton = false;

        //texts[(int)PauseButtonType.BGM_VOLUME - 1].text = "BGMVolume" + " < " + AudioManager.GetInstance().GetBGMMasterVolumeLevel() + " > ";
        //texts[(int)PauseButtonType.SE_VOLUME - 1].text = "SEVolume" + "      < " + AudioManager.GetInstance().GetSEMasterVolumeLevel() + " > ";
    }
    private void UpdateEffect()
    {
        timerEffect += Time.deltaTime;
        if (timerEffect >= 0.5f)
        {
            timerEffect = 0.5f;
        }

        //-80,-15
        //float circlePositionX = -texts[currentButtonIndex].text.Length / 2 * texts[currentButtonIndex].fontSize / 2 * texts[currentButtonIndex].gameObject.transform.localScale.x;
        float circlePositionY = texts[currentButtonIndex].transform.position.y;
        float underlinePositionY = texts[currentButtonIndex].gameObject.transform.position.y - texts[currentButtonIndex].fontSize / 2 * texts[currentButtonIndex].gameObject.transform.localScale.y;
        float adjustY = -20;

        circle.position = new Vector3(circle.position.x, circlePositionY);
        underLine.position = new Vector3(EaseOutExpo(405, 405 + 132 * 1.4f * 2.7f / 2, timerEffect / 0.5f), underlinePositionY + adjustY);
        underLine.localScale = new Vector3(EaseOutExpo(0, 1.4f, timerEffect / 0.5f), 0.35f);
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
        if (StartGameScene.IsGameStart())
        {
            GameTimeManager.GetInstance().SetTime(System.Convert.ToInt32(!isPause));
        }
    }

    public static bool IsPause()
    {
        return isPause;
    }

    float EaseOutExpo(float s, float e, float t)
    {
        if (t < 0) { t = 0; }
        else if (t > 1) { t = 1; }

        float v = t == 1 ? 1 : 1 - Mathf.Pow(2.0f, -10.0f * t);
        float a = e - s;
        v = s + a * v;

        return v;
    }

    public bool IsPause()
    {
        return isPause;
    }
}
