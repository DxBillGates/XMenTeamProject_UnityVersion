using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeEffect : MonoBehaviour
{

    [SerializeField] Material material;

    [Range(0,0.999f)] float threshold{ get; set; }

     [Range(0.001f,1)]float glowIntencity { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        threshold = 0.999f;
        glowIntencity = 0.056f;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_Threshold", threshold);
        material.SetFloat("_GlowCutoff", glowIntencity);
    }
}
