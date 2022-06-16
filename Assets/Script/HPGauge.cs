using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //�e�N����[�̔ԍ�
    int texNumber { get; set; }
    //�O������HP��ς���p�̕ϐ�
    public float hp { private get; set; }

    Slider hpSlider;

    // Start is cSalled before the first frame update
    void Start()
    {
        texNumber = 0;
        hp = 1.0f;

        hpSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        texNumber++;
        hpSlider.value = hp;
        //gameObject.GetComponent<Slider>().value = hp;
    }

}
