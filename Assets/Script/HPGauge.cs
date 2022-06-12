using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //テクすやーの番号
    int texNumber { get; set; }
    //外部からHPを変える用の変数
    float hp { set; get; }
    // Start is cSalled before the first frame update
    void Start()
    {
        texNumber = 0;
        hp = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        texNumber++;
        gameObject.GetComponent<Slider>().value = hp;
    }

}
