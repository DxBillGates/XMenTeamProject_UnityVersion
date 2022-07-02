using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateGauge : MonoBehaviour
{
    [SerializeField] List<Sprite>gaugeSprite;
    [SerializeField] CanvasGroup aura;
    //テクすやーの番号
    int texNumber { get; set; }

    private Image image;

    private float timerAlpha;

    // Start is cSalled before the first frame update
    void Start()
    {
        texNumber = 0;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetLevel(UltimateSkillManager.GetInstance().GetCurrentLevel());
        image.sprite = gaugeSprite[texNumber < gaugeSprite.Count ? texNumber : gaugeSprite.Count - 1];
        //gameObject.GetComponent<Image>().sprite = gaugeSprite[texNumber % gaugeSprite.Count];]

        //オーラの透明度
        timerAlpha += Time.deltaTime;
        if (timerAlpha >= 2)
        {
            timerAlpha -= 2;
        }

        if (texNumber == 4)
        {
            //ゲージMAX時
            aura.alpha = Mathf.Sin(timerAlpha * 90 * Mathf.Deg2Rad);
        }
        else
        {
            //MAXでないときは透明
            aura.alpha = 0;
        }
    }

    public void SetLevel(int setLevelValue)
    {
        texNumber = setLevelValue;
    }
}
