using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemyCollision : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.name == "Ball")
        {
            KnockBack(collision.gameObject.transform.position, collision);
        }
    }
}
