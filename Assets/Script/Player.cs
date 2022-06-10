using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movePower;
    private Vector3 velocity;

    [SerializeField] private Vector3 beforeDirection;
    [SerializeField] private Vector3 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3();

        Move();
        RotateDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                // �q�b�g������Q���̃q�b�g�����@�������ɉ����o���������炻�̖@�����擾
                Vector3 hitNormal = other.transform.forward;
                Vector3 pos = transform.position - velocity;

                // ���W���ʒu�t���[���O�ɖ߂�
                transform.position = pos;

                // �ǂ���x�N�g�����v�Z
                Vector3 moveVector = velocity - Vector3.Dot(velocity, hitNormal) * hitNormal;
                moveVector *= velocity.magnitude;
                transform.position += moveVector;
                break;
            case "Ball":
                break;
            case "Enemy":
                break;
        }

    }

    // �ړ�����
    private void Move()
    {
        beforeDirection = currentDirection;

        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isInput;

        velocity += movePower * Time.deltaTime * moveVector.normalized;

        transform.position += velocity;

        isInput = inputDirection.magnitude > 0.5f;
        if(isInput)currentDirection = inputDirection.normalized;
    }

    private void RotateDirection()
    {
        transform.rotation = Quaternion.LookRotation(currentDirection);
    }
}
