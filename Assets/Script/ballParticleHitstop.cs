using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballParticleHitstop : MonoBehaviour
{
    protected ParticleSystem particle;


    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var par = particle.main;
        par.simulationSpeed = GameTimeManager.GetInstance().GetTime();
    }
}
