using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movePower;
    [SerializeField] private GameObject ballObject;
    [SerializeField] private float throwBallCooldown;
    [SerializeField] private GameObject lineObjectManager;
    [SerializeField] private float maxHp;
    [SerializeField] private GameObject hpGaugeUIObject;

    private float hp;

    private Vector3 velocity;
    private Vector3 currentDirection;

    private Ball ballComponent;

    private bool isThrowBall;
    private float throwBallColldownTime;

    private PredictionalLineDrawer lineDrawer;

    private Vector3 knockbackVelocity;

    private HPGauge hpGauge;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        hp = maxHp;

        velocity = Vector3.zero;
        currentDirection = transform.forward;

        ballComponent = ballObject.GetComponent<Ball>();

        isThrowBall = false;
        throwBallColldownTime = 0;

        lineDrawer = lineObjectManager.GetComponent<PredictionalLineDrawer>();

        hpGauge = hpGaugeUIObject.GetComponent<HPGauge>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3();

        hpGauge.hp = CalclatePercentHP();

        UpdateAbilityCooldown();
        UpdateKnockback();

        Move();
        RotateDirection();

        HoldBallUpdate();
        ThrowingBall(currentDirection);


        lineDrawer.isDraw = ballComponent.state == BallState.HOLD_PLAYER;
        lineDrawer.drawOriginPosition = transform.position;
        lineDrawer.drawDirection = currentDirection;
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
                if (isThrowBall) break;
                if (ballComponent.state == BallState.THROWED_PLAYER || ballComponent.state == BallState.FREE)
                {
                    ballComponent.InitializeState(BallState.HOLD_PLAYER);
                }

                if (ballComponent.state == BallState.THROWED_ENEMY)
                {
                    Damage(ballComponent.GetSpeed());

                    Vector3 knockbackVector = (transform.position - other.gameObject.transform.position).normalized;
                    Knockback(knockbackVector * ballComponent.GetSpeed());
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

        velocity += movePower * moveVector.normalized;

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

        ballComponent.Throw(dir, BallState.THROWED_PLAYER);

        isThrowBall = true;
    }

    private void UpdateAbilityCooldown()
    {
        // �{�[���𓊂���N�[���_�E���X�V����
        if (throwBallColldownTime > throwBallCooldown)
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

    private void Knockback(Vector3 vector)
    {
        const float POWER = 2;
        knockbackVelocity = vector * POWER;
    }

    private void UpdateKnockback()
    {
        const float DECAY_VALUE = 2;

        Vector3 backupKnockbackVelocity = knockbackVelocity - knockbackVelocity / DECAY_VALUE;
        if(backupKnockbackVelocity.magnitude > knockbackVelocity.magnitude)
        {
            backupKnockbackVelocity = new Vector3();
            return;
        }

        velocity += backupKnockbackVelocity;
        knockbackVelocity -= backupKnockbackVelocity;
    }

    private void Damage(float value)
    {
        hp -= value;

        if (hp < 0) hp = 0;
    }

    private float CalclatePercentHP()
    {
        return hp / maxHp;
    }
}
