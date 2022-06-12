using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemy : Enemy
{
    // �v���C���[�Ƃ̊J���鋗��
    [SerializeField] private float playerToDistance = 10;
    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //�@��{�I�ȍs��
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        Vector3 moveVector = targetObject.transform.position - transform.position;

        // �ݒ肵��������艓����΋߂Â� �߂���Η����
        if (distance >= playerToDistance)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, targetObject.transform.position, moveSpeed);

        }
        else if (distance < playerToDistance - 1)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, -1 * moveVector + transform.position, moveSpeed);

        }
    }
}
