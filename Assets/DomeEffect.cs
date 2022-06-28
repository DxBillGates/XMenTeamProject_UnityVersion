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
        //0Б`0.0999В╠К╘В┼Т▓Ро
        threshold = 0.999f;
        //0.001Б`1В╠К╘В┼Т▓Ро
        glowIntencity = 0.056f;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Threshold", threshold);
        material.SetFloat("_GlowCutoff", glowIntencity);
    }
}
