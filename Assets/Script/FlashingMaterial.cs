using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 引数でもらったマテリアルを間隔や時間、色を指定して点滅させるクラス
[System.Serializable]
public class FlashingMaterial
{
    // 点滅色
    [SerializeField] private Color color;
    private Color originalColor;

    // 点滅間隔フラグ
    [SerializeField] private FlagController flashingSpanFlagController;
    //// 点滅時間フラグ
    //[SerializeField] private FlagController flashingFlagController;
    private FlagController externalFlagController;

    // 参照するマテリアル
    private Material refMaterial;
    private List<Color> setColors;
    private bool changeFlag;
    private bool isInitialized;

    public void Initialize()
    {
        flashingSpanFlagController.Initialize();
        //flashingFlagController.Initialize();

        if (setColors == null)
        {
            setColors = new List<Color>();
            setColors.Add(color);
            setColors.Add(new Color());
        }

        changeFlag = false;
        isInitialized = true;
    }

    public void Update(float deltaTime)
    {
        if (refMaterial == null) return;
        if (externalFlagController == null) return;

        //if (flashingFlagController.IsEndTrigger() == true)
        //{
        //    RestoreMaterialColor();
        //    Initialize();
        //    return;
        //}

        //if (flashingFlagController.flag == false) return;

        if (externalFlagController.IsEndTrigger() == true)
        {
            RestoreMaterialColor();
            Initialize();
            externalFlagController.Initialize();
            return;
        }

        if (externalFlagController.flag == false) return;


        // 点滅のフラグを管理しているフラグコントローラーが
        if (flashingSpanFlagController.flag == false) flashingSpanFlagController.flag = true;

        if(flashingSpanFlagController.IsEndTrigger() == true)
        {
            // 色変更に使うフラグを切り替える
            changeFlag = !changeFlag;

            // 要素番号0,1を切り替えれる
            refMaterial.color = setColors[System.Convert.ToInt32(changeFlag)];

            // 点滅フラグ管理を初期化
            flashingSpanFlagController.Initialize();
        }

        // 点滅フラグ管理の更新
        flashingSpanFlagController.Update(deltaTime);

        //// フラグ全体管理の更新
        //flashingFlagController.Update(deltaTime);
    }

    public void SetMaterial(Material material)
    {
        if (isInitialized == false) Debug.LogError("初期化処理が行われていません");

        refMaterial = material;
        originalColor = material.color;

        setColors[1] = originalColor;
    }

    // 点滅管理フラグをオンにする
    public void Flash(FlagController flagController)
    {
        flashingSpanFlagController.Initialize();
    }

    // マテリアルの色を戻す
    private void RestoreMaterialColor()
    {
        if (refMaterial == null) return;
        refMaterial.color = originalColor;
    }

    public void SetExternalFlagController(FlagController flagController)
    {
        externalFlagController = flagController;
    }
}
