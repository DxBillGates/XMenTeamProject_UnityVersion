using System.Collections;
using System.Collections.Generic;
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
        // �p�[�e�B�N�����I��������ɏ���
        if(particle.isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
