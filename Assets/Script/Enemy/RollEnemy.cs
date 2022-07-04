using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEnemy : Enemy
{
    // �v���C���[�Ƃ̊J���鋗��
    [SerializeField] private float playerToDistance = 10;
    // ��]���x
    [SerializeField] private float rollingSpeed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̏����i�[
        targetObject = GameObject.FindGameObjectWithTag("Player");

        // animetor
        GameObject temp = transform.root.Find("EnemyModel").gameObject;
        animator = temp.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Rolling();
    }

    // ��]�̋���
    protected void Rolling()
    {
       var rollY = rollingSpeed * GameTimeManager.GetInstance().GetTime();
        // �I�u�W�F�N�g����]������
        transform.Rotate(0, rollY, 0);

    }

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
        resultMoveVector *= GameTimeManager.GetInstance().GetTime();


        // �ݒ肵��������艓����΋߂Â� �߂���Η����
        if (distance > playerToDistance)
        {
            transform.position += resultMoveVector;
            movedVector = resultMoveVector;

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position -= resultMoveVector;
            movedVector = -resultMoveVector;
        }


    }

    private void OnTriggerStay(Collider collision)
    {
        // �{�[���ɓ��������Ƃ��̏���
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            // �{�[���ɓ��������Ƃ��̏���
            if (ballComponent.GetSpeed() > 0)
            {
                // �e�N���X�ł̏����p�ɕϐ��Ɋi�[
                hitBall = ballComponent;

                // �{�[���𓊂��Ԃ��������w��
                Vector3 throwVector = targetObject.transform.position - transform.position;
                throwVector.Normalize();
                throwVector.y = 0;

                float ballSpeed = collision.GetComponent<Ball>().GetSpeed();

                collision.GetComponent<Ball>().Throw(throwVector * ballSpeed, BallState.THROWED_ENEMY);

                KnockBack(collision);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            WallCollsion(collision.transform);
        }

    }
}
