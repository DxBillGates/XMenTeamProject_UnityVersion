using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    // ノックバックの速度
    [SerializeField] protected float knock_back_speed = 0.5f;
    // 移動速度
    [SerializeField] protected float moveSpeed = 2.0f;

    // hp
    [SerializeField] protected float hp = 10;

    // 敵同士で近づかない距離
    [SerializeField] protected float dontHitDistance = 3.0f;

    [SerializeField] protected List<AudioClip> SE;

    //player入れる
    protected GameObject targetObject;

    // 現フレームの移動量
    protected Vector3 movedVector;

    //SEのGetComponent用
    protected SEPlayManager sePlayManagerComponent;

    // アニメーションの速度変更用
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //試しに消した
        //PlayerFollow();
    }

    // 移動用関数
    protected virtual void Move()
    {

    }

    // 死亡時関数
    protected virtual void Defaat()
    {
    }

    protected void PlayerFollow()
    {
        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();
        moveVector.y = 0;

        // 離れるベクトルを計算
        Vector3 leaveV = new Vector3(0, 0, 0);

        // 敵同士での反発するベクトルを計算
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
            // 指定した範囲より敵が近い時に離れる
            if (distance <= dontHitDistance)
            {
                Vector3 calcLeaveV = transform.position - enemys[i].transform.position;

                calcLeaveV.y = 0;
                leaveV += calcLeaveV.normalized;
            }
        }
        leaveV.Normalize();

        // 壁との判定用に値を保存
        movedVector = (moveVector + leaveV) * moveSpeed;
        movedVector.y = 0;

        // 移動後のポジションを第二引数に
        transform.position += movedVector * GameTimeManager.GetInstance().GetTime();

        // 移動時 向き変更
        // プレイヤーの方向ベクトルを取得し、それを使い回転させる
        Vector3 playerV = targetObject.transform.position - transform.position;
        playerV.y = 0;

        if (playerV != Vector3.zero) transform.rotation = Quaternion.LookRotation(playerV);
    }


    /// <summary>
    /// 何かに当たったときのノックバック処理
    /// </summary>
    /// <param name="">当たった敵の位置</param>
    public void KnockBack(Collider collision)
    {
        Vector3 hitPos = collision.gameObject.transform.position;
        // ノックバックする位置を決める
        Vector3 moveVector = -1 * (hitPos - transform.position);
        // 正規化させる
        moveVector = knock_back_speed * moveVector.normalized;

        transform.position += moveVector;

        movedVector += moveVector;

        Damage(collision.gameObject.GetComponent<Ball>().GetSpeed());
    }

    protected void Damage(float damage)
    {
        hp -= damage;

        sePlayManagerComponent.SESeting(SE[0]);

        if (hp < 0)
        {
            EnemyManager.DecrementAliveCount();
            Destroy(transform.root.gameObject);
            const float ADD_GAUGE_VALUE = 25;

            // 倒したときにヒットストップ
            HitStopManager.GetInstance().HitStop();

            // スキルを発動していないときに敵を倒したならスキルゲージが増える
            if (UltimateSkillManager.GetInstance().IsUse() == false)
            {
                UltimateSkillManager.GetInstance().AddGauge(ADD_GAUGE_VALUE);
            }
        }
    }

    public void WallCollsion(Transform wallTransform)
    {
        // ヒットした障害物のヒットした法線方向に押し出したいからその法線を取得
        Vector3 hitNormal = wallTransform.forward;

        // 座標を位置フレーム前に戻す
        const float PUSH_VALUE = 3.0f;
        transform.position -= movedVector * PUSH_VALUE;

        // 壁ずりベクトルを計算
        Vector3 moveVector = movedVector - Vector3.Dot(movedVector, hitNormal) * hitNormal;
        transform.position += moveVector;
    }
}
