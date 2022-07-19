using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemyCollision : MonoBehaviour
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
        //�e�I�u�W�F�N�g�擾
        GameObject enemyParent = transform.root.gameObject;
        BarrierEnemy barrierEnemy = enemyParent.GetComponent<BarrierEnemy>();

        if (collision.gameObject.tag == "Wall")
        {
            //�e�Őe�̃g�����X�t�H�[���C��
            barrierEnemy.WallCollsion(collision.transform);
        }

        if (collision.gameObject.CompareTag("Pin"))
        {
            barrierEnemy.PinCollision(collision.transform);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //�e�I�u�W�F�N�g�擾
        GameObject enemyParent = transform.root.gameObject;
        BarrierEnemy barrierEnemy = enemyParent.GetComponent<BarrierEnemy>();

        // �{�[���ɓ��������Ƃ��̏���
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            if (ballComponent.GetSpeed() > 0)
            {
                barrierEnemy.HitBall = ballComponent;

                //�e�Ŕ���
                barrierEnemy.KnockBack(collision);
            }
        }
    }
}
