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
        if (collision.gameObject.tag == "Ball")
        {
            KnockBack(collision.gameObject.transform.position, collision);
        }

        if(collision.gameObject.tag == "Wall")
        {
            // �q�b�g������Q���̃q�b�g�����@�������ɉ����o���������炻�̖@�����擾
            Vector3 hitNormal = collision.transform.forward;

            // ���W���ʒu�t���[���O�ɖ߂�
            const float PUSH_VALUE = 3.0f;
            transform.position -= movedVector * PUSH_VALUE;

            // �ǂ���x�N�g�����v�Z
            Vector3 moveVector = movedVector - Vector3.Dot(movedVector, hitNormal) * hitNormal;
            transform.position += moveVector;
        }
    }
}
