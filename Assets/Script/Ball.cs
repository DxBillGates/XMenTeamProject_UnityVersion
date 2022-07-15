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
    // ボールを投げる際の強さ
    public float throwPower;
    // 減衰力
    public float attenuationPower;
    // 限界速度
    public float maxSpeed;
    // プレイヤーバリア反射時の加速度
    public float accelerateValue;
    // Enemy反射時の加速度
    public float enemyAccelerateValue;
    // ドームにあたった際の加速度
    public float domeHitAccelerateValue;
    // ドーム発動時の加速度
    public float domeTriggerAccelerationValue;
    // バリアと反射する際の反射率0 ~ 1で0に近いと法線をつかった反射ベクトルの計算に近づく
    public float barrierReflectance;
}

public class Ball : MonoBehaviour
{
    // Unityエディターから参照可能なボールの情報
    [SerializeField] private SerializableBallInfo ballInfo;
    //BarrierHitEffect.csをもつオブジェクト
    [SerializeField] private GameObject BarrierHitEffectManager;
    //SEリソース達
    [SerializeField] private List<AudioClip> SE;
    // ボールの状態を表すための色マテリアル
    [SerializeField] private List<Material> stateMaterials;

    private MeshRenderer meshRenderer;

    public BallState state { get; private set; }
    private Vector3 velocity;

    [SerializeField]private bool isThrow;
    [SerializeField]private bool isHitWall;
    [SerializeField]private bool isHitDome;
    [SerializeField]private bool isInDome;

    //軌跡
    private GameObject trail;
    private TrailRenderer trailRenderer;
    private bool trailFlg;

    private Collider collider;

    void Start()
    {
        // state velocity isThrowを初期化
        InitializeState(BallState.HOLD_PLAYER);

        // meshRendererを取得して現在のstateの色マテリアルをセットする
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
                // 敵が跳ね返したボールなら反射
                if (state == BallState.THROWED_ENEMY)
                {
                    hitNormal = (other.gameObject.transform.position - transform.position).normalized;
                    Reflection(hitNormal);
                }
                break;
            case "Enemy":
                // 投げられた状態でそのボールが動いていれば
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

                // 押し出し
                pushValue = CollisionManager.CollisionBoxAndPlane(transform, collider.bounds, other.transform, hitNormal);
                dotNormalAndVelocity = Vector3.Dot(velocity, hitNormal);

                transform.position += pushValue * dotNormalAndVelocity * velocity.normalized;
                break;

            // プレイヤーのバリアにあたったときの処理
            case "Barrier":
                // 投げられた状態でそのボールが動いていれば
                if (isThrow == false && velocity.magnitude <= 0) break;

                transform.position += pushValue * dotNormalAndVelocity * velocity.normalized;

                Reflection(hitNormal, true,true);
                // 速度ベクトルの大きさを取得
                float speed = velocity.magnitude;

                Vector3 newVelocity = Vector3.Lerp(velocity, other.gameObject.transform.forward * speed, ballInfo.barrierReflectance);
                velocity = newVelocity;

                state = BallState.THROWED_PLAYER;
                AudioManager.GetInstance().PlayAudio(SE[0], MyAudioType.SE, audioVolume, false);

                //ヒット時エフェクト
                BarrierHitEffectManager.GetComponent<BarrierHitEffect>().Use(gameObject.transform.position);
                HitStopManager.GetInstance().HitStop();

                break;
            case "EnemyBarrier":
                // 投げられた状態でそのボールが動いていれば
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


    // ゲームオブジェクトにその向きの速度を与える
    public void Throw(Vector3 direction, BallState setState)
    {
        velocity = direction * ballInfo.throwPower;

        Vector3 backupPosition = transform.position;
        backupPosition.y = 0;
        transform.position = backupPosition;

        isThrow = true;
        state = setState;
    }

    // ボールの移動処理
    private void Move()
    {
        // 速度ベクトルを減衰させる
        velocity -= velocity.normalized * ballInfo.attenuationPower * GameTimeManager.GetInstance().GetTime();

        // 減衰したあとに既定値より速度ベクトルの大きさが小さいなら止まっているみなす
        const float MIN_VELOCITY = 0.01f;
        if (velocity.magnitude < MIN_VELOCITY) InitializeState(BallState.FREE);
        if (velocity.magnitude > ballInfo.maxSpeed) velocity = velocity.normalized * ballInfo.maxSpeed;

        // 速度ベクトルにゲーム内時間を掛ける
        Vector3 resultVelocity = velocity * GameTimeManager.GetInstance().GetTime();
        resultVelocity.y = 0;

        // 現在のベクトルが壁のサイズを超えたときにレイキャストを行い壁をすり抜けないようにベクトルの大きさを制限する
        resultVelocity = DontPenetrater.CalcVelocity(transform.position, resultVelocity);
        // 速度上限
        if (resultVelocity.magnitude > ballInfo.maxSpeed) resultVelocity = resultVelocity.normalized * ballInfo.maxSpeed;

        transform.position += resultVelocity;

        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        // ドーム内に一度でも入っているなら速度ベクトルを足したときに外に出ないように調整
        if (isInDome == false) return;
        if (Vector3.Distance(transform.position, UltimateSkillManager.GetInstance().usedPosition) > UltimateSkillManager.GetInstance().usedSize - transform.localScale.x)
        {
            transform.position -= resultVelocity * GameTimeManager.GetInstance().GetTime();
        }

    }

    // 反射ベクトルを生成
    public void Reflection(Vector3 normal, bool enemy = false, bool addSpeed = false)
    {
        Vector3 backupVelocity = velocity * GameTimeManager.GetInstance().GetTime();
        Vector3 reflectVector = backupVelocity - 2.0f * Vector3.Dot(backupVelocity, normal) * normal;

        // 法線ベクトルとの内積を計算して鈍角なら反射をせずに終了させる
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

        // 引数のaddSpeedFlagの内容で反射ベクトルに加速度をかけるか
        velocity *= addSpeed ? acc : 1;

        // 加速後の速度が上限を超え内容制限
        if (velocity.magnitude > ballInfo.maxSpeed) velocity = velocity.normalized * ballInfo.maxSpeed;
    }

    // ボールの状態を初期化
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

    // ドームとの当たり判定
    private void UpdateDomeDetection()
    {
        UltimateSkillManager ultimateSkillManager = UltimateSkillManager.GetInstance();
        FlagController flagController = ultimateSkillManager.GetActiveFlagController();
        FlagActiveType flagActiveType = flagController.activeType;


        isInDome = flagController.isEnd;
        if (ultimateSkillManager.IsUse() == false) return;

        // 必殺技発動前なら発動地点に持ってくる
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

            // ドームとの距離がドームの半径を超えているなら反射
            if (distace + transform.localScale.x / 2 > ultimateSkillManager.usedSize)
            {
                //transform.position -= velocity.normalized * Mathf.Abs(distanceSubject);

                // 法線と速度ベクトルで内積を取り鈍角（速度ベクトルの方向が法線側）なら反射させない
                if (Vector3.Dot(hitNormal, velocity) > 0)
                {
                    return;
                }

                Reflection(hitNormal.normalized, false, true);
                isHitDome = true;
            }
        }
    }

    // ドーム発動時に加速させるための関数
    public void AddTriggerSkillAcc()
    {
        velocity += velocity.normalized * ballInfo.domeTriggerAccelerationValue;
        //velocity *= ballInfo.domeTriggerAccelerationValue;
    }


    // ボールのパーティクル用に最大速度を取ってくる
    public float GetMaxSpeed()
    {
        return ballInfo.maxSpeed;
    }
}
