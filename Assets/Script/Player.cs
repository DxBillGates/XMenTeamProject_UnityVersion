using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movePower;
    [SerializeField] private GameObject ballObject;
    [SerializeField] private float throwBallCooldown;

    private Vector3 velocity;
    private Vector3 currentDirection;

    private Ball ballComponent;

    private bool isThrowBall;
    private float throwBallColldownTime;

    // Start is called before the first frame update
    void Start()
    {
        ballComponent = ballObject.GetComponent<Ball>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3();

        UpdateAbilityCooldown();

        Move();
        RotateDirection();

        HoldBallUpdate();
        ThrowingBall(currentDirection);
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Wall":
                // �q�b�g������Q���̃q�b�g�����@�������ɉ����o���������炻�̖@�����擾
                Vector3 hitNormal = other.transform.forward;

                // ���W���ʒu�t���[���O�ɖ߂�
                const float PUSH_VALUE = 1.5f;
                transform.position -= velocity * PUSH_VALUE;

                // �ǂ���x�N�g�����v�Z
                Vector3 moveVector = velocity - Vector3.Dot(velocity, hitNormal) * hitNormal;
                transform.position += moveVector;
                break;
            case "Ball":
                // �{�[������������������ԂȂ�ێ���ԂɕύX
                if (isThrowBall == true) break;
                if (ballComponent.state == BallState.THROWED_PLAYER || ballComponent.state == BallState.FREE)
                {
                    ballComponent.InitializeState(BallState.HOLD_PLAYER);
                }
                break;
            case "Enemy":
                break;
        }

    }

    // �ړ�����
    private void Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isInput;

        velocity = movePower * moveVector.normalized;

        transform.position += velocity;

        isInput = inputDirection.magnitude > 0.5f;
        if (isInput) currentDirection = inputDirection.normalized;
    }

    // �v���C���[���ړ��x�N�g�������Ɍ�������
    private void RotateDirection()
    {
        if (currentDirection.magnitude == 0) return;

        transform.rotation = Quaternion.LookRotation(currentDirection);
    }

    private void ThrowingBall(Vector3 dir)
    {
        // ������L�[�������Ă��Ȃ� or �{�[����ێ����Ă��Ȃ��ꍇ�͏��������Ȃ�
        if (isThrowBall == true) return;
        if (Input.GetButtonDown("PlayerAbility") == false) return;
        if (ballComponent.state != BallState.HOLD_PLAYER) return;

        ballComponent.Throw(dir,BallState.THROWED_PLAYER);

        isThrowBall = true;
    }

    private void UpdateAbilityCooldown()
    {
        // �{�[���𓊂���N�[���_�E���X�V����
        if(throwBallColldownTime > throwBallCooldown)
        {
            isThrowBall = false;
            throwBallColldownTime = 0;
        }
        if (isThrowBall) throwBallColldownTime += Time.deltaTime;
    }

    // �{�[����ێ����Ă���Ƃ��Ƀ{�[���������̉E��O�ɔz�u
    private void HoldBallUpdate()
    {
        if (isThrowBall == true) return;
        if (ballComponent.state != BallState.HOLD_PLAYER) return;

        ballComponent.transform.position = transform.position + transform.right;
    }
}
