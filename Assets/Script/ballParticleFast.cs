using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ballParticleFast : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particleObj;

    private GameObject ball;



    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");

    }

    // Update is called once per frame
    void Update()
    {

        float speed = ball.GetComponent<Ball>().GetSpeed();
        float maxSpeed = ball.GetComponent<Ball>().GetMaxSpeed();



        // スピードが最大スピードの半分以上の時に
        if (speed >= maxSpeed / 2)
        {
            var particlePosition = ball.transform.position;

            var createdParticle = Instantiate(particleObj,particlePosition,Quaternion.identity);
            createdParticle.GetComponent<ParticleSystem>().startColor = ball.GetComponent<Renderer>().material.color;

        }

    }

}
