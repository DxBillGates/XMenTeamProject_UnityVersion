using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrierEnemy : Enemy
{
    // �v���C���[�Ƃ̊J���鋗��
    [SerializeField] private float playerToDistance = 10;
    // �����ύX�̃t���[��
    [SerializeField] private int change_pose_frame = 120;



    // �o���A�̏��i�[�p
    [SerializeField] private GameObject barrier;

    private Vector3[] ballDir;

    // �����ύX�p
    private int ballCurrentNum = 0; // �{�[���̏�������ԍ�
    private int ballBeforeNum = 0; // �����ύX�p�̔ԍ�
    private bool firstCountflg = false;// �ŏ��A�z�������܂ł̊֐�

    private bool barrierDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        Array.Resize(ref ballDir, change_pose_frame);

        // �O�̂��߃^�O�t��
        transform.tag = "Enemy";
    }

    // Update is called once per frame
    void Update ()
    {
        // �{�[���̈ʒu���i�[
        SetBallDir();
        // �o���A������ł���̂��m�F
        if (!barrierDestroy) CheckBarrierDown();

        ChangePose();

        if (!barrierDestroy)
        {
            Move();
        }
        else
        {
            PlayerFollow();

        }
    }

    //�@��{�I�ȍs��
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        // �ړ��x�N�g���̌v�Z
        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();

        // �G�̈ʒu�ɔz����������

        Vector3 leaveV = new Vector3(0, 0, 0);

        // �G���m�ł̔�������x�N�g�����v�Z
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            float distanceEnemy = Vector3.Distance(transform.position, enemys[i].transform.position);
            // �w�肵���͈͂��G���߂����ɗ����
            if (distanceEnemy <= dontHitDistance)
            {
                // �����x�N�g�����v�Z y������x�l�����Ȃ��ł���
                Vector3 calcLeaveV = transform.position - enemys[i].transform.position;
                calcLeaveV.y = 0;
                leaveV += calcLeaveV.normalized;
            }

        }
        leaveV.Normalize();

        Vector3 resultMoveVector = (moveVector + leaveV) * moveSpeed;
        resultMoveVector.y = 0;

        // �ݒ肵��������艓����΋߂Â� �߂���Η����
        if (distance > playerToDistance)
        {
            transform.position += resultMoveVector;
            movedVector = (moveVector + leaveV) * moveSpeed;

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position -= resultMoveVector;
            movedVector = -((moveVector + leaveV) * moveSpeed);
        }


    }

    private void ChangePose()
    {
        if (firstCountflg)
        {
            // 0�x�N�g���ł͂Ȃ��Ƃ��ɑ��
            if(ballDir[ballBeforeNum] != Vector3.zero)transform.rotation = Quaternion.LookRotation(ballDir[ballBeforeNum]);

            ++ballBeforeNum;
            if(ballBeforeNum >= ballDir.Length)
            {
                ballBeforeNum = 0;
            }
        }
        else
        {
            if(ballDir[0] != Vector3.zero)transform.rotation = Quaternion.LookRotation(ballDir[0]);
        }

    }

    private void SetBallDir()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");

        // �{�[���ւ̃x�N�g�����v�Z
        Vector3 ballVector = ball.transform.position - transform.position;

        // �ϐ��Ɋi�[
        ballDir[ballCurrentNum] = ballVector.normalized;
        ++ballCurrentNum;

        if(ballCurrentNum >= ballDir.Length)
        {
            ballCurrentNum = 0;
            firstCountflg = true;
        }
    }


    private void CheckBarrierDown()
    {
        if(barrier == null)
        {
            barrierDestroy = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.tag == "Ball")
        {
            KnockBack(collision.gameObject.transform.position, collision);
        }

        if (collision.gameObject.tag == "Wall")
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
