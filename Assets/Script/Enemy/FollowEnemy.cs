using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        // �O�̂��߃^�O�t��
        transform.tag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFollow();

        ChangePose();
    }

    private void ChangePose()
    {
        // �v���C���[�̕����x�N�g�����擾���A������g����]������
        Vector3 playerV = targetObject.transform.position - transform.position;
        playerV.y = 0;

        if(playerV != Vector3.zero)transform.rotation = Quaternion.LookRotation(playerV);
    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.tag == "Ball")
        {
            KnockBack(collision.gameObject.transform.position,collision);
        }
        else if(collision.gameObject.tag == "Wall")
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
