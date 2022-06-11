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

    // 移動処理
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

        ballComponent.Throw(dir,BallState.THROWED_PLAYER);

        isThrowBall = true;
    }

    private void UpdateAbilityCooldown()
    {
        // ボールを投げるクールダウン更新処理
        if(throwBallColldownTime > throwBallCooldown)
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
}
