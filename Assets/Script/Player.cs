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

    //バリアを展開するスクリプト
    private BarrierControl barrierControl;

    private PredictionalLineDrawer lineDrawer;

    private Vector3 knockbackVelocity;

    private HPGauge hpGauge;

    private Animator animator;
    private Vector3 triggerSkillPosition;

    [SerializeField] private float hitEnemyDamage;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        hp = maxHp;

        velocity = Vector3.zero;
        currentDirection = transform.forward;

        ballComponent = ballObject.GetComponent<Ball>();
        barrierControl = GetComponent<BarrierControl>();

        isThrowBall = false;
        throwBallColldownTime = 0;

        lineDrawer = lineObjectManager.GetComponent<PredictionalLineDrawer>();

        hpGauge = hpGaugeUIObject.GetComponent<HPGauge>();

        animator = GetComponent<Animator>();
        triggerSkillPosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //バリアの展開する向き指定
        barrierControl.direction = gameObject.transform.forward;

        velocity = new Vector3();

        hpGauge.hp = CalclatePercentHP();

        UpdateAbilityCooldown();
        UpdateKnockback();

        bool isMove = Move();
        RotateDirection();

        HoldBallUpdate();
        ThrowingBall(currentDirection);
        UsingBarrier();
        UseUltimateSkill();

        UpdateAnimator(isMove);
        UpdateUsingSkillPosition();

        lineDrawer.isDraw = ballComponent.state == BallState.HOLD_PLAYER;
        lineDrawer.drawOriginPosition = transform.position;
        lineDrawer.drawDirection = currentDirection;
    }

    private void OnTriggerStay(Collider other)
    {
        // 必殺技使用中なら当たり判定を行わない
        if (UltimateSkillManager.GetInstance().GetActiveFlagController().flag == true) return;

        switch (other.gameObject.tag)
        {
            case "Wall":
                // ヒットした障害物のヒットした法線方向に押し出したいからその法線を取得
                Vector3 hitNormal = other.transform.forward;

                // 座標を位置フレーム前に戻す
                const float PUSH_VALUE = 1.5f;
                transform.position -= velocity * PUSH_VALUE;

                // 壁ずりベクトルを計算
                Vector3 moveVector = velocity - Vector3.Dot(velocity, hitNormal) * hitNormal;
                transform.position += moveVector;
                break;
            case "Ball":
                // ボールが自分が投げた状態なら保持状態に変更
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

    private void OnTriggerEnter(Collider other)
    {
        // 必殺技使用中なら当たり判定を行わない
        if (UltimateSkillManager.GetInstance().GetActiveFlagController().flag == true) return;

        switch (other.gameObject.tag)
        {
            case "Wall":
                break;
            case "Ball":
                break;
            case "Enemy":
                Damage(hitEnemyDamage);

                Vector3 knockbackVector = (transform.position - other.gameObject.transform.position).normalized;
                Knockback(knockbackVector * 3);
                break;
        }
    }

    // 移動処理
    private bool Move()
    {
        Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        bool isInput;

        velocity += movePower * moveVector.normalized;

        transform.position += velocity;

        isInput = inputDirection.magnitude > 0.5f;
        if (isInput) currentDirection = inputDirection.normalized;

        return isInput;
    }

    // プレイヤーを移動ベクトル方向に向かせる
    private void RotateDirection()
    {
        if (currentDirection.magnitude == 0) return;

        transform.rotation = Quaternion.LookRotation(currentDirection);
    }

    private void ThrowingBall(Vector3 dir)
    {
        // 投げるキーを押していない or ボールを保持していない場合は処理をしない
        if (isThrowBall == true) return;
        if (Input.GetButtonDown("PlayerAbility") == false) return;
        if (ballComponent.state != BallState.HOLD_PLAYER) return;

        ballComponent.Throw(dir, BallState.THROWED_PLAYER);

        isThrowBall = true;
    }

    private void UsingBarrier()
    {
        // ボールを投げたばかりの状態ならリターン
        if (isThrowBall == true) return;
        // ボールを保持しているならリターン
        if (ballComponent.state == BallState.HOLD_PLAYER) return;
        // アビリティキーを押していないならリターン
        if (Input.GetButtonDown("PlayerAbility") == false) return;

        barrierControl.Use();
    }

    private void UseUltimateSkill()
    {
        if (Input.GetButtonDown("UltimateSkill") == false) return;

        const float DIVIDE = 3;
        Vector3 upVector = new Vector3(0, transform.localScale.y / DIVIDE, 0);
        bool isUseSkill = UltimateSkillManager.GetInstance().Use(transform.position, currentDirection + upVector, transform);

        if (isUseSkill) triggerSkillPosition = transform.position;
    }

    private void UpdateAbilityCooldown()
    {
        // ボールを投げるクールダウン更新処理
        if (throwBallColldownTime > throwBallCooldown)
        {
            isThrowBall = false;
            throwBallColldownTime = 0;
        }
        if (isThrowBall) throwBallColldownTime += Time.deltaTime;
    }

    // ボールを保持しているときにボールを自分の右手前に配置
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
        if (backupKnockbackVelocity.magnitude > knockbackVelocity.magnitude)
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

    private void UpdateAnimator(bool isMoveInput)
    {
        if (animator == null) return;
        bool isHoldBall = ballComponent.state == BallState.HOLD_PLAYER ? true : false;
        bool isHoldBallMove = isHoldBall && isMoveInput;
        bool isUseSkill = CameraMotionManager.GetInstance().IsAnimationTime();

        animator.SetBool("UltimateSkill", isUseSkill);

        if (isUseSkill == true) return;

        animator.SetBool("BallHaveWalk", isMoveInput & isHoldBall);
        animator.SetBool("Walk", isMoveInput && isHoldBall == false);
    }

    private void UpdateUsingSkillPosition()
    {
        UltimateSkillManager ultimateSkillManager = UltimateSkillManager.GetInstance();

        if (ultimateSkillManager.GetActiveFlagController().activeType == FlagActiveType.END)
        {
            FlagController endSkillFlagController = ultimateSkillManager.GetActiveFlagController();
            if (endSkillFlagController.IsEndTrigger() == true)
            {
                transform.position = triggerSkillPosition;
            }
        }

        // スキル使用中じゃない場合は即リターン
        if (ultimateSkillManager.IsUse() == false) return;
        if (ultimateSkillManager.GetActiveFlagController().activeType != FlagActiveType.ACTIVE) return;

        UltimateSkillGenerator ultimateSkillGenerator = UltimateSkillGenerator.GetInstance();
        transform.position = ultimateSkillGenerator.GetCreatedObjectPosition() + Vector3.up * ultimateSkillGenerator.GetCreatedObjectScale();
    }
}
