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

        if(playerV != Vector3.zero)transform.rotation = Quaternion.LookRotation(playerV);
    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.name == "Ball")
        {
            KnockBack(collision.gameObject.transform.position,collision);
        }
    }
}
