using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    //�e�N����[�̔ԍ�
    int texNumber { get; set; }
    //�O������HP��ς���p�̕ϐ�
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
