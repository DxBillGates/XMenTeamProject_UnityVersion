using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeParticleControl : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //particleSystem.Stop();
    }
}
