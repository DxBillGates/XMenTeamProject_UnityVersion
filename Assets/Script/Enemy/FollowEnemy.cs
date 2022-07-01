using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        // animetor
        GameObject temp = transform.root.Find("EnemyModel").gameObject;
        animator = temp.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFollow();

        // �q�b�g�X�g�b�v���Ƀ��f���̃A�j���[�V�������X�g�b�v������
        animator.SetFloat("Speed",GameTimeManager.GetInstance().GetTime());
        //ChangePose();
    }

    private void ChangePose()
    {
        // �v���C���[�̕����x�N�g�����擾���A������g����]������
        Vector3 playerV = targetObject.transform.position - transform.position;
        playerV.y = 0;

        if (playerV != Vector3.zero) transform.rotation = Quaternion.LookRotation(playerV);
    }

    private void OnTriggerStay(Collider collision)
    {
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            // �{�[���ɓ��������Ƃ��̏���
            if (ballComponent.GetSpeed() > 0)
            {
                KnockBack(collision);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            WallCollsion(collision.transform);
        }

    }
}
