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
    // ���E���x
    [SerializeField] private float maxSpeed;
    // ���ˎ��̉����x
    [SerializeField] private float accelerateValue;
    [SerializeField] private List<Material> stateMaterials;

    private MeshRenderer meshRenderer;

    private Vector3 velocity;
    private bool isThrow;
    public BallState state { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        InitializeState(BallState.THROWED_PLAYER);
        Throw(new Vector3(0.5f, 0, 1),BallState.THROWED_PLAYER);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = stateMaterials[(int)state];
    }

    private void FixedUpdate()
    {
        meshRenderer.material = stateMaterials[(int)state];
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrow) Move();

        UpdateDomeDetection();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitNormal = other.gameObject.transform.forward;

        switch (other.gameObject.tag)
        {
            case "Player":
                // �G�����˕Ԃ����{�[���Ȃ����������
                if (state == BallState.THROWED_ENEMY)
                {
                    hitNormal = (other.gameObject.transform.position - transform.position).normalized;
                    Reflection(hitNormal);
                }
                break;
            case "Enemy":
                // ������ꂽ��Ԃł��̃{�[���������Ă����
                if (isThrow == false && velocity.magnitude <= 0) break;

                hitNormal = (other.gameObject.transform.position - transform.position).normalized;
                Reflection(hitNormal, true);

                state = BallState.THROWED_ENEMY;
                break;
            case "Wall":
                Reflection(hitNormal);
                break;
            case "Barrier":
                Reflection(hitNormal, true);
                state = BallState.THROWED_PLAYER;
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

        transform.position += velocity * GameTimeManager.GetInstance().GetTime();
    }

    // ���˃x�N�g���𐶐�
    private void Reflection(Vector3 normal,bool addSpeed = false)
    {
        Vector3 reflectVector = velocity - 2.0f * Vector3.Dot(velocity, normal) * normal;
        velocity = reflectVector;

        velocity = addSpeed ? reflectVector * accelerateValue : reflectVector;

        if (velocity.magnitude > maxSpeed) velocity = velocity.normalized * maxSpeed;
    }

    // �{�[���̏�Ԃ�������
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

    // �h�[���Ƃ̓����蔻��
    private void UpdateDomeDetection()
    {
        UltimateSkillManager ultimateSkillManager = UltimateSkillManager.GetInstance();
        FlagController flagController = ultimateSkillManager.GetActiveFlagController();
        FlagActiveType flagActiveType = flagController.activeType;

        // �K�E�Z�����O�Ȃ甭���n�_�Ɏ����Ă���
        if (flagActiveType == FlagActiveType.PRE)
        {
            //transform.position = ultimateSkillManager.usedPosition;
            float perTime = flagController.activeTime / flagController.maxActiveTime;
            transform.position = Vector3.Lerp(transform.position, ultimateSkillManager.usedPosition, perTime);
        }
        else if (flagActiveType == FlagActiveType.ACTIVE)
        {
            Vector3 hitNormal = transform.position - ultimateSkillManager.usedPosition;
            float distace = hitNormal.magnitude;
            if(distace > ultimateSkillManager.usedSize)
            {
                Reflection(hitNormal.normalized, true);
            }
        }
    }
}