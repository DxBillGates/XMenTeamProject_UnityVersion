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

    private Vector3[] ballDir;

    // �����ύX�p
    private int ballCurrentNum = 0; // �{�[���̏�������ԍ�
    private int ballBeforeNum = 0; // �����ύX�p�̔ԍ�
    private bool firstCountflg = false;// �ŏ��A�z�������܂ł̊֐�

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

        ChangePose();

        Move();
    }

    //�@��{�I�ȍs��
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();



        // �ݒ肵��������艓����΋߂Â� �߂���Η����
        if (distance >= playerToDistance)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, moveVector + transform.position, moveSpeed);

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, -1 * moveVector + transform.position, moveSpeed);

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


   

}
