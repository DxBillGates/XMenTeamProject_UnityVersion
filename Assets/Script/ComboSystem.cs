using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboSystem
{
    // 現在のコンボ数
    [SerializeField] private int combo;
    // コンボが何回一定回数に到達したか
    private int comboCount;

    // 何コンボ毎に大きさをあげるか
    [SerializeField, Range(1, 10)] private int increaseSizeNCombo;
    // 一定コンボ毎にどれだけ大きくさせるか
    [SerializeField, Range(1, 10)] private float increaseSize;
    // 大きくさせるオブジェクトの最大サイズ
    [SerializeField] private float maxSize;

    // コンボ時に大きくさせるオブジェクト
    private GameObject ballObject;
    private Vector3 initSize;

    [SerializeField] private List<UnityEngine.UI.Image> images;
    [SerializeField] private List<Sprite> numTextures;
    [SerializeField] private UnityEngine.UI.Image prefabImage;

    // Start is called before the first frame update
    public void Initialize()
    {
        combo = 0;
        comboCount = 0;
        initSize = Vector3.zero;

        //// Inspectorでセットされていない場合は自分でUIを作成する
        //if (images.Count == 0)
        //{
        //    // 三桁分作成
        //    images = new List<UnityEngine.UI.Image>();

        //    for (int i = 0; i < 3; ++i)
        //    {
        //        var image = GameObject.Instantiate(prefabImage);
        //        image.rectTransform.SetParent(GameObject.Find("Canvas").transform);
        //        images.Add(image);
        //    }
        //}
    }

    // Update is called once per frame
    public void Update()
    {
        if (images.Count == 0) return;

        int startNumber = 1000;
        int startNumber2 = 100;
        foreach(var image in images)
        {
            int textureIndex = 0;
            textureIndex = combo == 0 ? 0 : combo % startNumber / startNumber2;
            image.sprite = numTextures[textureIndex];
            startNumber /= 10;
            startNumber2 /= 10;
        }
    }

    public void SetBallObject(GameObject argObject)
    {
        ballObject = argObject;
        initSize = argObject.transform.localScale;
    }

    public void IncreaseCombo()
    {
        ++combo;

        // コンボがボールが大きくなるために必要なコンボ数ずつ判定させる
        if (combo > increaseSizeNCombo + increaseSizeNCombo * comboCount)
        {
            ++comboCount;
            ballObject.transform.localScale += Vector3.one * increaseSize;

            // ここにUIが大きくなり始めるような処理を記述


            // オブジェクトのサイズを増加させる
            if (ballObject.transform.localScale.x >= maxSize)
            {
                ballObject.transform.localScale = Vector3.one * maxSize;
            }
        }
    }

    public void ResetCombo()
    {
        combo = 0;
        comboCount = 0;
        ballObject.transform.localScale = initSize;
    }
}
