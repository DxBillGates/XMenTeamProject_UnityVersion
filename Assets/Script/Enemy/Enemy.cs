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
        transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, moveSpeed);
    }


    /// <summary>
    /// �����ɓ��������Ƃ��̃m�b�N�o�b�N����
    /// </summary>
    /// <param name="">���������G�̈ʒu</param>
    protected void KnockBack(Vector3 hitPos)
    {
        // �m�b�N�o�b�N����ʒu�����߂�
        Vector3 moveVector = -1 * (hitPos - transform.position);
        // ���K��������
        moveVector = knock_back_speed * moveVector.normalized;

        transform.position =
            Vector3.MoveTowards(transform.position, moveVector + transform.position, knock_back_speed);
    }

    private void OnTriggerStay(Collider collision)
    {

        // �{�[���ɓ��������Ƃ��̏���
        if (collision.gameObject.name == "Ball")
        {
            KnockBack(collision.transform.position);
        }

    }

}
