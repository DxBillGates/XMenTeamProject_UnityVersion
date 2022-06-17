using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    // �m�b�N�o�b�N�̑��x
    [SerializeField] protected float knock_back_speed = 0.5f;
    // �ړ����x
    [SerializeField] protected float moveSpeed = 2.0f;

    // hp
    [SerializeField] protected float hp = 10;

    // �G���m�ŋ߂Â��Ȃ�����
    [SerializeField] protected float dontHitDistance = 3.0f;

    //player�����
    protected GameObject targetObject;

    // ���t���[���̈ړ���
    protected Vector3 movedVector;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //�����ɏ�����
        //PlayerFollow();
    }

    // �ړ��p�֐�
    protected virtual void Move()
    {

    }

    // ���S���֐�
    protected virtual void Defaat()
    {
    }

    protected void PlayerFollow()
    {
        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();
        moveVector.y = 0;

        // �����x�N�g�����v�Z
        Vector3 leaveV = new Vector3(0, 0, 0);

        // �G���m�ł̔�������x�N�g�����v�Z
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0;i<enemys.Length;i++)
        {
            float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
            // �w�肵���͈͂��G���߂����ɗ����
            if (distance <= dontHitDistance)
            {
                Vector3 calcLeaveV = transform.position - enemys[i].transform.position;

                calcLeaveV.y = 0;
                leaveV += calcLeaveV.normalized;
            }
        }
        leaveV.Normalize();

        // �ǂƂ̔���p�ɒl��ۑ�
        movedVector = (moveVector + leaveV) * moveSpeed;
        movedVector.y = 0;

        // �ړ���̃|�W�V�������������
        transform.position += movedVector;
    }


    /// <summary>
    /// �����ɓ��������Ƃ��̃m�b�N�o�b�N����
    /// </summary>
    /// <param name="">���������G�̈ʒu</param>
    protected void KnockBack(Vector3 hitPos, Collider collision)
    {
        // �m�b�N�o�b�N����ʒu�����߂�
        Vector3 moveVector = -1 * (hitPos - transform.position);
        // ���K��������
        moveVector = knock_back_speed * moveVector.normalized;

        transform.position += moveVector;

        movedVector += moveVector;

        Damage(collision.gameObject.GetComponent<Ball>().GetSpeed());
    }

    protected void Damage(float damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            Destroy(transform.root.gameObject);
            const float ADD_GAUGE_VALUE = 25;
            UltimateSkillManager.GetInstance().AddGauge(ADD_GAUGE_VALUE);
        }
    }
}
