using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    FREE,
    HOLD_PLAYER,
    HOLD_ENEMY,
    THROWED_PLAYER,
    THROWED_ENEMY,
}



public class Ball : MonoBehaviour
{
    // ボールを投げる際の強さ
    [SerializeField] private float throwPower;
    // 減衰力
    [SerializeField] private float attenuationPower;
    // 限界速度
    [SerializeField] private float maxSpeed;
    // 反射時の加速度
    [SerializeField] private float accelerateValue;

    private Vector3 velocity;
    private bool isThrow;
    public BallState state { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        InitializeState(BallState.THROWED_PLAYER);
        Throw(new Vector3(0.5f, 0, 1),BallState.THROWED_PLAYER);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (isThrow) Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitNormal = other.gameObject.transform.forward;

        switch (other.gameObject.tag)
        {
            case "Player":
                // 敵が跳ね返したボールなら加速＆反射
                if (state == BallState.THROWED_ENEMY) Reflection(hitNormal, true);
                break;
            case "Enemy":
                state = BallState.THROWED_ENEMY;
                break;
            case "Wall":
                Reflection(hitNormal);
                break;
            case "Barrier":
                Debug.Log(hitNormal);
                Reflection(hitNormal, true);
                break;
        }
    }

    // ゲームオブジェクトをその向きに対する速度を与える
    public void Throw(Vector3 direction,BallState setState)
    {
        velocity = direction * throwPower;

        isThrow = true;
        state = setState;
    }

    // ボールの移動処理
    private void Move()
    {
        velocity -= velocity.normalized * attenuationPower;

        const float MIN_VELOCITY = 0.01f;
        if (velocity.magnitude < MIN_VELOCITY) InitializeState(BallState.FREE);

        transform.position += velocity;
    }

    // 反射ベクトルを生成
    private void Reflection(Vector3 normal,bool addSpeed = false)
    {
        Vector3 reflectVector = velocity - 2.0f * Vector3.Dot(velocity, normal) * normal;
        velocity = reflectVector;

        velocity = addSpeed ? reflectVector * accelerateValue : reflectVector;
    }

    // ボールの状態を初期化
    public void InitializeState(BallState setState)
    {
        velocity = Vector3.zero;
        isThrow = false;
        state = setState;
    }

    public float GetSpeed()
    {
        return velocity.magnitude;
    }
}
