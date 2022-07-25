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
    EXIT,
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
    [SerializeField] private Image bgmVolume_10;
    [SerializeField] private Image seVolume;
    [SerializeField] private Image seVolume_10;
    [SerializeField] private Image bgmArrowLeft;
    [SerializeField] private Image bgmArrowRight;
    [SerializeField] private Image seArrowLeft;
    [SerializeField] private Image seArrowRight;
    [SerializeField] private List<Sprite> sprNum;
    [SerializeField] private NextScene sceneChange;

    // ポーズ中の操作が何秒後から可能か
    [SerializeField] private float enabledTime;

    // ポーズ中の経過時間
    private float pauseTime;

    // 球と下線の演出用タイマー
    private float timerEffect = 0;

    //オレンジ
    Color32 COLOR_ORANGE = new Color32(242, 145, 25, 255);

    //青
    Color32 COLOR_BLUE = new Color32(32, 174, 227, 255);

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void OnDisable()
    {
        isPause = false;
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
            case PauseButtonType.EXIT:
                OnClickExit();
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

            AudioManager.GetInstance().PlaySelectSE();
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
        int beforeButtonIndex = currentButtonIndex;

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

        if (beforeButtonIndex != currentButtonIndex) AudioManager.GetInstance().PlaySelectSE();
    }

    // 選択されているUIの色を変更する
    private void UpdateUIColor()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            if (i == currentButtonIndex)
            {
                texts[i].color = COLOR_ORANGE;
            }
            else
            {
                texts[i].color = COLOR_BLUE;
            }
        }
        //BGM数字
        if (currentButtonIndex == (int)PauseButtonType.BGM_VOLUME)
        {
            bgmVolume.color = COLOR_ORANGE;
            bgmVolume_10.color = COLOR_ORANGE;
        }
        else
        {
            bgmVolume.color = COLOR_BLUE;
            bgmVolume_10.color = COLOR_BLUE;
        }
        //SE数字
        if (currentButtonIndex == (int)PauseButtonType.SE_VOLUME)
        {
            seVolume.color = COLOR_ORANGE;
            seVolume_10.color = COLOR_ORANGE;
        }
        else
        {
            seVolume.color = COLOR_BLUE;
            seVolume_10.color = COLOR_BLUE;
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
        int bgm = AudioManager.GetInstance().GetBGMMasterVolumeLevel();
        int se = AudioManager.GetInstance().GetSEMasterVolumeLevel();
        if (bgm < 10)
        {
            bgmVolume.sprite = sprNum[AudioManager.GetInstance().GetBGMMasterVolumeLevel()];
            bgmVolume_10.sprite = sprNum[0];
        }
        else
        {
            bgmVolume.sprite = sprNum[0];
            bgmVolume_10.sprite = sprNum[1];
        }
        if (se < 10)
        {
            seVolume.sprite = sprNum[AudioManager.GetInstance().GetSEMasterVolumeLevel()];
            seVolume_10.sprite = sprNum[0];
        }
        else
        {
            seVolume.sprite = sprNum[0];
            seVolume_10.sprite = sprNum[1];
        }
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

        const float BEFORE_POS_X = 435;
        const float AFTER_POS_X = BEFORE_POS_X + 132 * 1.2f * 2.7f / 2;
        const float BEFORE_SCALE = 0;
        const float AFTER_SCALE = 1.2f;

        circle.position = new Vector3(circle.position.x, circlePositionY);
        underLine.position = new Vector3(EaseOutExpo(BEFORE_POS_X, AFTER_POS_X, timerEffect / 0.5f), underlinePositionY + adjustY);
        underLine.localScale = new Vector3(EaseOutExpo(BEFORE_SCALE, AFTER_SCALE, timerEffect / 0.5f), 0.35f);
    }

    // タイトルへのUIを押した際に実行する内容
    private void OnClickTitle()
    {
        sceneChange.nextSceneName = "TitleScene";
        sceneChange.gameObject.SetActive(true);
    }

    // リスタートのUIを押した際に実行する内容
    private void OnClickRestart()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        sceneChange.nextSceneName = currentScene.name;
        sceneChange.gameObject.SetActive(true);
    }

    // ステージセレクトのUIを押した際に実行する内容
    private void OnClickStageSelect()
    {
        sceneChange.nextSceneName = "StageSelectScene";
        sceneChange.gameObject.SetActive(true);
    }

    // ExitのUIを押した際に実行する内容
    private void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
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
}
