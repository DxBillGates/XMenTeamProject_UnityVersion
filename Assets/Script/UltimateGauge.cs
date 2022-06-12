using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateGauge : MonoBehaviour
{
    [SerializeField] List<Sprite>gaugeSprite;
    //ÉeÉNÇ∑Ç‚Å[ÇÃî‘çÜ
    int texNumber { get; set; }
    // Start is cSalled before the first frame update
    void Start()
    {
        texNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Image>().sprite = gaugeSprite[texNumber % gaugeSprite.Count];
    }
}
