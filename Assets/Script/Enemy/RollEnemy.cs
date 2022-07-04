using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEnemy : Enemy
{
    // プレイヤーとの開ける距離
    [SerializeField] private float playerToDistance = 10;
    // 回転速度
    [SerializeField] private float rollingSpeed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーの情報を格納
        targetObject = GameObject.FindGameObjectWithTag("Player");

        // animetor
        GameObject temp = transform.root.Find("EnemyModel").gameObject;
        animator = temp.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Rolling();
    }

    // 回転の挙動
    protected void Rolling()
    {
       var rollY = rollingSpeed * GameTimeManager.GetInstance().GetTime();
        // オブジェクトを回転させる
        transform.Rotate(0, rollY, 0);

    }

    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        // 移動ベクトルの計算
        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();

        // 敵の位置に配慮した動き

        Vector3 leaveV = new Vector3(0, 0, 0);

        // 敵同士での反発するベクトルを計算
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            float distanceEnemy = Vector3.Distance(transform.position, enemys[i].transform.position);
            // 指定した範囲より敵が近い時に離れる
            if (distanceEnemy <= dontHitDistance)
            {
                // 離れるベクトルを計算 y軸を一度考慮しないでおく
                Vector3 calcLeaveV = transform.position - enemys[i].transform.position;
                calcLeaveV.y = 0;
                leaveV += calcLeaveV.normalized;
            }

        }
        leaveV.Normalize();

        Vector3 resultMoveVector = (moveVector + leaveV) * moveSpeed;
        resultMoveVector.y = 0;
        resultMoveVector *= GameTimeManager.GetInstance().GetTime();


        // 設定した距離より遠ければ近づく 近ければ離れる
        if (distance > playerToDistance)
        {
            transform.position += resultMoveVector;
            movedVector = resultMoveVector;

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position -= resultMoveVector;
            movedVector = -resultMoveVector;
        }


    }

    private void OnTriggerStay(Collider collision)
    {
        // ボールに当たったときの処理
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            // ボールに当たったときの処理
            if (ballComponent.GetSpeed() > 0)
            {
                // 親クラスでの処理用に変数に格納
                hitBall = ballComponent;

                // ボールを投げ返す方向を指定
                Vector3 throwVector = targetObject.transform.position - transform.position;
                throwVector.Normalize();
                throwVector.y = 0;

                float ballSpeed = collision.GetComponent<Ball>().GetSpeed();

                collision.GetComponent<Ball>().Throw(throwVector * ballSpeed, BallState.THROWED_ENEMY);

                KnockBack(collision);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            WallCollsion(collision.transform);
        }

    }
}
