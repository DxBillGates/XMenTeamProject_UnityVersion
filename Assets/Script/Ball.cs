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
    // �{�[���𓊂���ۂ̋���
    [SerializeField] private float throwPower;
    // ������
    [SerializeField] private float attenuationPower;

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
        Reflection(hitNormal);

        switch (other.gameObject.tag)
        {
            case "Player":
                break;
            case "Enemy":
                state = BallState.THROWED_ENEMY;
                break;
        }
    }

    // �Q�[���I�u�W�F�N�g�����̌����ɑ΂��鑬�x��^����
    public void Throw(Vector3 direction,BallState setState)
    {
        velocity = direction * throwPower;

        isThrow = true;
        state = setState;
    }

    // �{�[���̈ړ�����
    private void Move()
    {
        velocity -= velocity.normalized * attenuationPower;

        const float MIN_VELOCITY = 0.01f;
        if (velocity.magnitude < MIN_VELOCITY) InitializeState(BallState.FREE);

        transform.position += velocity;
    }

    // ���˃x�N�g���𐶐�
    private void Reflection(Vector3 normal)
    {
        Vector3 reflectVector = velocity - 2.0f * Vector3.Dot(velocity, normal) * normal;
        velocity = reflectVector;
    }

    // �{�[���̏�Ԃ�������
    public void InitializeState(BallState setState)
    {
        velocity = Vector3.zero;
        isThrow = false;
        state = setState;
    }
}
