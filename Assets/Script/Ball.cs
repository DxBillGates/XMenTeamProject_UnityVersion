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

[System.Serializable]
public struct SerializableBallInfo
{
    // �{�[���𓊂���ۂ̋���
    public float throwPower;
    // ������
    public float attenuationPower;
    // ���E���x
    public float maxSpeed;
    // �v���C���[�o���A���ˎ��̉����x
    public float accelerateValue;
    // Enemy���ˎ��̉����x
    public float enemyAccelerateValue;
    // �h�[���ɂ��������ۂ̉����x
    public float domeHitAccelerateValue;
    // �h�[���������̉����x
    public float domeTriggerAccelerationValue;
    // �o���A�Ɣ��˂���ۂ̔��˗�0 ~ 1��0�ɋ߂��Ɩ@�������������˃x�N�g���̌v�Z�ɋ߂Â�
    public float barrierReflectance;
}

public class Ball : MonoBehaviour
{
    // Unity�G�f�B�^�[����Q�Ɖ\�ȃ{�[���̏��
    [SerializeField] private SerializableBallInfo ballInfo;
    //BarrierHitEffect.cs�����I�u�W�F�N�g
    [SerializeField] private GameObject BarrierHitEffectManager;
    //SE���\�[�X�B
    [SerializeField] private List<AudioClip> SE;
    // �{�[���̏�Ԃ�\�����߂̐F�}�e���A��
    [SerializeField] private List<Material> stateMaterials;

    private MeshRenderer meshRenderer;

    public BallState state { get; private set; }
    private Vector3 velocity;

    [SerializeField]private bool isThrow;
    [SerializeField]private bool isHitWall;
    [SerializeField]private bool isHitDome;
    [SerializeField]private bool isInDome;

    //�O��
    private GameObject trail;
    private TrailRenderer trailRenderer;
    private bool trailFlg;

    private Collider collider;

    void Start()
    {
        // state velocity isThrow��������
        InitializeState(BallState.HOLD_PLAYER);

        // meshRenderer���擾���Č��݂�state�̐F�}�e���A�����Z�b�g����
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = stateMaterials[(int)state];

        isHitWall = false;
        isHitDome = false;
        isInDome = false;

        trail = transform.GetChild(0).gameObject;
        trailRenderer = trail.GetComponent<TrailRenderer>();
        trailFlg = false;
        trail.SetActive(trailFlg);

        collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        meshRenderer.material = stateMaterials[(int)state];
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitNormal = other.gameObject.transform.forward;

        const float audioVolume = 0.5f;

        float pushValue = CollisionManager.CollisionBoxAndPlane(transform, collider.bounds, other.transform, hitNormal);
        float dotNormalAndVelocity = Vector3.Dot(velocity, hitNormal);

        switch (other.gameObject.tag)
        {
            case "Player":
                // �G�����˕Ԃ����{�[���Ȃ甽��
                if (state == BallState.THROWED_ENEMY)
                {
                    hitNormal = (other.gameObject.transform.position - transform.position).normalized;
                    Reflection(hitNormal);
                }
                break;
            case "Enemy":
                // ������ꂽ��Ԃł��̃{�[���������Ă����
                if (isThrow == false && velocity.magnitude <= 0) break;

                hitNormal = -(other.gameObject.transform.position - transform.position).normalized;
                Reflection(hitNormal, true, true);
                state = BallState.THROWED_ENEMY;
                break;
            case "Wall":
                if (isHitDome == true)
                {
                    isHitDome = false;
                    break;
                }
                isHitWall = true;
                Reflection(hitNormal);
                AudioManager.GetInstance().PlayAudio(SE[0], MyAudioType.SE, audioVolume, false);

                // �����o��
                pushValue = CollisionManager.CollisionBoxAndPlane(transform, collider.bounds, other.transform, hitNormal);
                dotNormalAndVelocity = Vector3.Dot(velocity, hitNormal);

                transform.position += pushValue * dotNormalAndVelocity * velocity.normalized;
                break;

            // �v���C���[�̃o���A�ɂ��������Ƃ��̏���
            case "Barrier":
                // ������ꂽ��Ԃł��̃{�[���������Ă����
                if (isThrow == false && velocity.magnitude <= 0) break;

                transform.position += pushValue * dotNormalAndVelocity * velocity.normalized;

                Reflection(hitNormal, true,true);
                // ���x�x�N�g���̑傫�����擾
                float speed = velocity.magnitude;

                Vector3 newVelocity = Vector3.Lerp(velocity, other.gameObject.transform.forward * speed, ballInfo.barrierReflectance);
                velocity = newVelocity;

                state = BallState.THROWED_PLAYER;
                AudioManager.GetInstance().PlayAudio(SE[0], MyAudioType.SE, audioVolume, false);

                //�q�b�g���G�t�F�N�g
                BarrierHitEffectManager.GetComponent<BarrierHitEffect>().Use(gameObject.transform.position);
                HitStopManager.GetInstance().HitStop();

                break;
            case "EnemyBarrier":
                // ������ꂽ��Ԃł��̃{�[���������Ă����
                if (isThrow == false && velocity.magnitude <= 0) break;
                if (state == BallState.HOLD_PLAYER) break;
                Reflection(hitNormal, true);
                state = BallState.THROWED_ENEMY;
                AudioManager.GetInstance().PlayAudio(SE[0], MyAudioType.SE, audioVolume, false);
                break;
        }
    }

    void Update()
    {
        UpdateDomeDetection();
        if (isThrow) Move();
        TrailController();

        isHitDome = false;
        isHitWall = false;
    }


    // �Q�[���I�u�W�F�N�g�ɂ��̌����̑��x��^����
    public void Throw(Vector3 direction, BallState setState)
    {
        velocity = direction * ballInfo.throwPower;

        Vector3 backupPosition = transform.position;
        backupPosition.y = 0;
        transform.position = backupPosition;

        isThrow = true;
        state = setState;
    }

    // �{�[���̈ړ�����
    private void Move()
    {
        // ���x�x�N�g��������������
        velocity -= velocity.normalized * ballInfo.attenuationPower * GameTimeManager.GetInstance().GetTime();

        // �����������ƂɊ���l��葬�x�x�N�g���̑傫�����������Ȃ�~�܂��Ă���݂Ȃ�
        const float MIN_VELOCITY = 0.01f;
        if (velocity.magnitude < MIN_VELOCITY) InitializeState(BallState.FREE);
        if (velocity.magnitude > ballInfo.maxSpeed) velocity = velocity.normalized * ballInfo.maxSpeed;

        // ���x�x�N�g���ɃQ�[�������Ԃ��|����
        Vector3 resultVelocity = velocity * GameTimeManager.GetInstance().GetTime();
        resultVelocity.y = 0;

        // ���݂̃x�N�g�����ǂ̃T�C�Y�𒴂����Ƃ��Ƀ��C�L���X�g���s���ǂ����蔲���Ȃ��悤�Ƀx�N�g���̑傫���𐧌�����
        resultVelocity = DontPenetrater.CalcVelocity(transform.position, resultVelocity);
        // ���x���
        if (resultVelocity.magnitude > ballInfo.maxSpeed) resultVelocity = resultVelocity.normalized * ballInfo.maxSpeed;

        transform.position += resultVelocity;

        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        // �h�[�����Ɉ�x�ł������Ă���Ȃ瑬�x�x�N�g���𑫂����Ƃ��ɊO�ɏo�Ȃ��悤�ɒ���
        if (isInDome == false) return;
        if (Vector3.Distance(transform.position, UltimateSkillManager.GetInstance().usedPosition) > UltimateSkillManager.GetInstance().usedSize - transform.localScale.x)
        {
            transform.position -= resultVelocity * GameTimeManager.GetInstance().GetTime();
        }

    }

    // ���˃x�N�g���𐶐�
    public void Reflection(Vector3 normal, bool enemy = false, bool addSpeed = false)
    {
        Vector3 backupVelocity = velocity * GameTimeManager.GetInstance().GetTime();
        Vector3 reflectVector = backupVelocity - 2.0f * Vector3.Dot(backupVelocity, normal) * normal;

        // �@���x�N�g���Ƃ̓��ς��v�Z���ē݊p�Ȃ甽�˂������ɏI��������
        float dotReflectAndNormal = Vector3.Dot(reflectVector, normal);
        float dotReflectAndNormalAngle = 180.0f * dotReflectAndNormal / Mathf.PI;
        if (Mathf.Abs(dotReflectAndNormalAngle) > 180) return;

        velocity = reflectVector.normalized * velocity.magnitude;

        float acc;
        if (UltimateSkillManager.GetInstance().IsActiveFlagControllerFlag())
        {
            acc = ballInfo.domeHitAccelerateValue;
        }
        else if (enemy)
        {
            acc = ballInfo.enemyAccelerateValue;
        }
        else
        {
            acc = ballInfo.accelerateValue;
        }

        // ������addSpeedFlag�̓��e�Ŕ��˃x�N�g���ɉ����x�������邩
        velocity *= addSpeed ? acc : 1;

        // ������̑��x������𒴂����e����
        if (velocity.magnitude > ballInfo.maxSpeed) velocity = velocity.normalized * ballInfo.maxSpeed;
    }

    // �{�[���̏�Ԃ�������
    public void InitializeState(BallState setState)
    {
        state = setState;
        velocity = Vector3.zero;
        isThrow = false;
    }

    public float GetSpeed()
    {
        return velocity.magnitude;
    }

    private void TrailController()
    {
        switch (state)
        {
            case BallState.FREE:
            case BallState.HOLD_ENEMY:
            case BallState.HOLD_PLAYER:
                if (trailFlg)
                {
                    trailFlg = false;
                    trail.SetActive(trailFlg);
                }
                break;
            case BallState.THROWED_ENEMY:
            case BallState.THROWED_PLAYER:
                if (!trailFlg)
                {
                    trailFlg = true;
                    trail.SetActive(trailFlg);
                }
                trailRenderer.startColor = stateMaterials[(int)state].color;
                Color color = stateMaterials[(int)state].color;
                color.a = 0.01f;
                trailRenderer.endColor = color;

                break;

        }
    }

    // �h�[���Ƃ̓����蔻��
    private void UpdateDomeDetection()
    {
        UltimateSkillManager ultimateSkillManager = UltimateSkillManager.GetInstance();
        FlagController flagController = ultimateSkillManager.GetActiveFlagController();
        FlagActiveType flagActiveType = flagController.activeType;


        isInDome = flagController.isEnd;
        if (ultimateSkillManager.IsUse() == false) return;

        // �K�E�Z�����O�Ȃ甭���n�_�Ɏ����Ă���
        if (flagActiveType == FlagActiveType.PRE)
        {
            //transform.position = ultimateSkillManager.usedPosition;
            float perTime = flagController.activeTime / flagController.maxActiveTime;
            Vector3 setPos = Vector3.Lerp(transform.position, ultimateSkillManager.usedPosition, perTime);

            if (Vector3.Distance(setPos, ultimateSkillManager.usedPosition) > ultimateSkillManager.usedSize - transform.localScale.x * 2)
            {
                transform.position = setPos;
                Vector3 vector = ultimateSkillManager.usedPosition - transform.position;
                vector = vector.normalized * velocity.magnitude;
                velocity = Vector3.Lerp(velocity, vector, 0.7f);
            }
            else
            {
                isInDome = true;
            }
        }
        if (flagActiveType == FlagActiveType.ACTIVE || flagActiveType == FlagActiveType.END || isInDome)
        {
            if (isHitWall == true)
            {
                isHitWall = false;
                return;
            }
            Vector3 hitNormal = -(transform.position - ultimateSkillManager.usedPosition);
            float distace = hitNormal.magnitude;
            float distanceSubject = ultimateSkillManager.usedSize - (distace + transform.localScale.x / 2);

            // �h�[���Ƃ̋������h�[���̔��a�𒴂��Ă���Ȃ甽��
            if (distace + transform.localScale.x / 2 > ultimateSkillManager.usedSize)
            {
                //transform.position -= velocity.normalized * Mathf.Abs(distanceSubject);

                // �@���Ƒ��x�x�N�g���œ��ς����݊p�i���x�x�N�g���̕������@�����j�Ȃ甽�˂����Ȃ�
                if (Vector3.Dot(hitNormal, velocity) > 0)
                {
                    return;
                }

                Reflection(hitNormal.normalized, false, true);
                isHitDome = true;
            }
        }
    }

    // �h�[���������ɉ��������邽�߂̊֐�
    public void AddTriggerSkillAcc()
    {
        velocity += velocity.normalized * ballInfo.domeTriggerAccelerationValue;
        //velocity *= ballInfo.domeTriggerAccelerationValue;
    }


    // �{�[���̃p�[�e�B�N���p�ɍő呬�x������Ă���
    public float GetMaxSpeed()
    {
        return ballInfo.maxSpeed;
    }
}
