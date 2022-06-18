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

        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.tag == "Ball")
        {
            //�e�Ŕ���
            barrierEnemy.KnockBack(collision);
        }

        if (collision.gameObject.tag == "Wall")
        {
            //�e�Őe�̃g�����X�t�H�[���C��
            barrierEnemy.WallCollsion(collision.transform);
        }
    }
}
