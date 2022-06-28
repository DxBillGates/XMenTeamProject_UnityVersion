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
    //SE鳴らすマネージャーオブジェクト
    [SerializeField] private GameObject SEPlayManager;

    //SEリソース達
    [SerializeField] private List<AudioClip> SE;

    // ボールを投げる際の強さ
    [SerializeField] private float throwPower;
    // 減衰力
    [SerializeField] private float attenuationPower;
    // 限界速度
    [SerializeField] private float maxSpeed;
    // プレイヤーバリア反射時の加速度
    [SerializeField] private float accelerateValue;

    // Enemy反射時の加速度
    [SerializeField] private float enemyAccelerateValue;

    // ドームにあたった際の加速度
    [SerializeField] private float domeHitAccelerateValue;
    [SerializeField] private List<Material> stateMaterials;

    // バリアと反射する際の反射率0 ~ 1で0に近いと法線をつかった反射ベクトルの計算に近づく
    [SerializeField, Range(0, 1)] private float barrierReflectance;

    private MeshRenderer meshRenderer;

    //GetConponent用
    private SEPlayManager sePlayManager;

    private Vector3 velocity;
    private bool isThrow;
    public BallState state { get; private set; }

    private bool isHitWall;
    private bool isHitDome;
    private bool isInDome;

    //軌跡
    private GameObject trail;
    private TrailRenderer trailRenderer;
    private bool trailFlg;

    // Start is called before the first frame update
    void Start()
    {
        InitializeState(BallState.THROWED_PLAYER);
        Throw(new Vector3(0.5f, 0, 1), BallState.THROWED_PLAYER);

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = stateMaterials[(int)state];

        isHitWall = false;
        isHitDome = false;
        isInDome = false;

        sePlayManager = SEPlayManager.GetComponent<SEPlayManager>();

        trail = transform.GetChild(0).gameObject;
        trailRenderer = trail.GetComponent<TrailRenderer>();
        trailFlg = false;
        trail.SetActive(trailFlg);
    }

    private void FixedUpdate()
    {
        meshRenderer.material = stateMaterials[(int)state];
    }

    // Update is called once per frame
    void Update()
    {
        isHitDome = false;

        if (isThrow) Move();
        TrailController();
        UpdateDomeDetection();
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitNormal = other.gameObject.transform.forward;

        const float audioVolume = 0.5f;

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
                Reflection(hitNormal,true, true);
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
                sePlayManager.SESeting(SE[0], audioVolume);
                break;
            case "Barrier":
                // 投げられた状態でそのボールが動いていれば
                if (isThrow == false && velocity.magnitude <= 0) break;

                Reflection(hitNormal, true);
                // 速度ベクトルの大きさを取得
                float speed = velocity.magnitude;

                Vector3 newVelocity = Vector3.Lerp(velocity, other.gameObject.transform.forward * speed,barrierReflectance);
                velocity = newVelocity;

                state = BallState.THROWED_PLAYER;
                sePlayManager.SESeting(SE[0], audioVolume);
                break;
            case "EnemyBarrier":
                // 投げられた状態でそのボールが動いていれば
                if (isThrow == false && velocity.magnitude <= 0) break;
                if (state == BallState.HOLD_PLAYER) break;
                Reflection(hitNormal, true);
                state = BallState.THROWED_ENEMY;
                sePlayManager.SESeting(SE[0], audioVolume);
                break;
        }
    }


    // ゲームオブジェクトをその向きに対する速度を与える
    public void Throw(Vector3 direction, BallState setState)
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

        Vector3 resultVelocity = velocity * GameTimeManager.GetInstance().GetTime();
        resultVelocity.y = 0;

        transform.position += resultVelocity * GameTimeManager.GetInstance().GetTime();

        // ドーム内に一度でも入っているなら速度ベクトルを足したときに外に出ないように調整
        if (isInDome == false) return;
        if (Vector3.Distance(transform.position, UltimateSkillManager.GetInstance().usedPosition) > UltimateSkillManager.GetInstance().usedSize - transform.localScale.x)
        {
            transform.position -= resultVelocity * GameTimeManager.GetInstance().GetTime();
        }

        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
    }

    // 反射ベクトルを生成
    private void Reflection(Vector3 normal, bool enemy = false, bool addSpeed = false)
    {
        Vector3 backupVelocity = velocity;
        Vector3 reflectVector = velocity - 2.0f * Vector3.Dot(velocity, normal) * normal;

        // 法線ベクトルとの内積を計算して鈍角なら反射をせずに終了させる
        float dotReflectAndNormal = Vector3.Dot(reflectVector, normal);
        float dotReflectAndNormalAngle = 180.0f * dotReflectAndNormal / Mathf.PI;
        if (Mathf.Abs(dotReflectAndNormalAngle) > 180) return;

        velocity = reflectVector;

        float acc = 0.0f;

        if (UltimateSkillManager.GetInstance().IsUse())
        {
            acc = domeHitAccelerateValue;
        }
        else if (enemy)
        {
            acc = enemyAccelerateValue;
        }
        else
        {
            acc = accelerateValue;
        }
		
		// 引数のaddSpeedFlagの内容で反射ベクトルに加速度をかけるか
        velocity = addSpeed ? reflectVector * acc : reflectVector;

		// 加速後の速度が上限を超え内容制限
        if (velocity.magnitude > maxSpeed) velocity = velocity.normalized * maxSpeed;
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

        // 必殺技発動前なら発動地点に持ってくる
        if (flagActiveType == FlagActiveType.PRE)
        {
            //transform.position = ultimateSkillManager.usedPosition;
            float perTime = flagController.activeTime / flagController.maxActiveTime;
            Vector3 setPos = Vector3.Lerp(transform.position, ultimateSkillManager.usedPosition, perTime);

            if (Vector3.Distance(setPos, ultimateSkillManager.usedPosition) > ultimateSkillManager.usedSize - transform.localScale.x * 2)
            {
                transform.position = setPos;
            }
            else
            {
                isInDome = true;
            }
        }
        else if (flagActiveType == FlagActiveType.ACTIVE)
        {

            if (isHitWall == true)
            {
                isHitWall = false;
                return;
            }
            Vector3 hitNormal = -(transform.position - ultimateSkillManager.usedPosition);
            float distace = hitNormal.magnitude;
            float distanceSubject = distace - ultimateSkillManager.usedSize - transform.localScale.x;

            if (distace > ultimateSkillManager.usedSize - transform.localScale.x)
            {
                transform.position -= velocity.normalized * Mathf.Abs(distanceSubject);
                Reflection(hitNormal.normalized,false,true);
                isHitDome = true;
            }
        }
    }
}
