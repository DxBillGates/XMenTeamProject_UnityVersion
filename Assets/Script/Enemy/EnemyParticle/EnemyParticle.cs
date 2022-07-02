using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyParticleDestroy : MonoBehaviour
{
    // Start is called before the first frame update

    ParticleSystem particle;

    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        var particleArray = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();

        foreach(var par in particleArray)
        {
            // パーティクルの実行速度にヒットストップ適用
            var parMain = par.main;
            parMain.simulationSpeed = GameTimeManager.GetInstance().GetTime();


        }
        // パーティクルが終わった時に消す
        if(particle.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
