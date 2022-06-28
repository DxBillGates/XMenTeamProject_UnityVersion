using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeEffect : MonoBehaviour
{

    [SerializeField] Material material;

    float threshold { get; set; }

    float glowIntencity { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        //0�`0.0999�̊ԂŒ���
        threshold = 0.999f;
        //0.001�`1�̊ԂŒ���
        glowIntencity = 0.056f;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Threshold", threshold);
        material.SetFloat("_GlowCutoff", glowIntencity);
    }
}
