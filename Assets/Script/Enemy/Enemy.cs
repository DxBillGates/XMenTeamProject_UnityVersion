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

        // �G���m�ł̔�������x�N�g�����v�Z
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemys )
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            // �w�肵���͈͂��G���߂����ɗ����
            if(distance <= dontHitDistance)
            {
                // �����x�N�g�����v�Z
                Vector3 leaveV = transform.position - e.transform.position;

                moveVector += leaveV.normalized;
            }

        }

        // �ړ���̃|�W�V�������������
        transform.position = Vector3.MoveTowards(transform.position, moveVector + transform.position, moveSpeed);
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

        //Damage(collision.gameObject.GetComponent<Ball>().GetSpeed());
    }

    protected void Damage(float damage)
    {
        hp -= damage;

        if (hp < 0)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
