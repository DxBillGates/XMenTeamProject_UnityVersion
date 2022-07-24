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

    [SerializeField] private float movePower;
    [SerializeField] private float g;
    [SerializeField]private Vector3 velocity;
    private float yOffset;
    private bool upFlag;

    // Start is called before the first frame update
    public void Initialize()
    {
        combo = 0;
        comboCount = 0;
        initSize = Vector3.zero;

        if (images.Count == 0) return;
        yOffset = images[0].transform.position.y;
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

        UpdateUIPosition();
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
        if (combo >= increaseSizeNCombo + increaseSizeNCombo * comboCount)
        {
            ++comboCount;
            ballObject.transform.localScale += Vector3.one * increaseSize;

            // ここにUIが大きくなり始めるような処理を記述



            velocity.y = movePower;
            upFlag = true;

            // オブジェクトのサイズを増加を制限
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

    private void UpdateUIPosition()
    {
        if (upFlag == false) return;
        velocity.y -= g;
        foreach(var image in images)
        {
            image.rectTransform.position += velocity;
            if(image.rectTransform.position.y <= yOffset)
            {
                Vector3 pos = image.rectTransform.position;
                pos.y = yOffset;
                image.rectTransform.position = pos;
                upFlag = false;
            }
        }

        if(upFlag == false)
        {
            velocity = Vector3.zero;
        }
    }
}
