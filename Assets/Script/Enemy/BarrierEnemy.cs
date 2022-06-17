using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrierEnemy : Enemy
{
    // プレイヤーとの開ける距離
    [SerializeField] private float playerToDistance = 10;
    // 向き変更のフレーム
    [SerializeField] private int change_pose_frame = 120;



    // バリアの情報格納用
    [SerializeField] private GameObject barrier;

    private Vector3[] ballDir;

    // 向き変更用
    private int ballCurrentNum = 0; // ボールの情報を入れる番号
    private int ballBeforeNum = 0; // 向き変更用の番号
    private bool firstCountflg = false;// 最初、配列を入れるまでの関数

    private bool barrierDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        Array.Resize(ref ballDir, change_pose_frame);

        // 念のためタグ付け
        transform.tag = "Enemy";
    }

    // Update is called once per frame
    void Update ()
    {
        // ボールの位置を格納
        SetBallDir();
        // バリアが死んでいるのか確認
        if (!barrierDestroy) CheckBarrierDown();

        ChangePose();

        if (!barrierDestroy)
        {
            Move();
        }
        else
        {
            PlayerFollow();

        }
    }

    //　基本的な行動
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

        // 設定した距離より遠ければ近づく 近ければ離れる
        if (distance > playerToDistance)
        {
            transform.position += resultMoveVector;
            movedVector = (moveVector + leaveV) * moveSpeed;

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position -= resultMoveVector;
            movedVector = -((moveVector + leaveV) * moveSpeed);
        }


    }

    private void ChangePose()
    {
        if (firstCountflg)
        {
            // 0ベクトルではないときに代入
            if(ballDir[ballBeforeNum] != Vector3.zero)transform.rotation = Quaternion.LookRotation(ballDir[ballBeforeNum]);

            ++ballBeforeNum;
            if(ballBeforeNum >= ballDir.Length)
            {
                ballBeforeNum = 0;
            }
        }
        else
        {
            if(ballDir[0] != Vector3.zero)transform.rotation = Quaternion.LookRotation(ballDir[0]);
        }

    }

    private void SetBallDir()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");

        // ボールへのベクトルを計算
        Vector3 ballVector = ball.transform.position - transform.position;

        // 変数に格納
        ballDir[ballCurrentNum] = ballVector.normalized;
        ++ballCurrentNum;

        if(ballCurrentNum >= ballDir.Length)
        {
            ballCurrentNum = 0;
            firstCountflg = true;
        }
    }


    private void CheckBarrierDown()
    {
        if(barrier == null)
        {
            barrierDestroy = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        // ボールに当たったときの処理
        if (collision.gameObject.tag == "Ball")
        {
            KnockBack(collision.gameObject.transform.position, collision);
        }

        if (collision.gameObject.tag == "Wall")
        {
            // ヒットした障害物のヒットした法線方向に押し出したいからその法線を取得
            Vector3 hitNormal = collision.transform.forward;

            // 座標を位置フレーム前に戻す
            const float PUSH_VALUE = 3.0f;
            transform.position -= movedVector * PUSH_VALUE;

            // 壁ずりベクトルを計算
            Vector3 moveVector = movedVector - Vector3.Dot(movedVector, hitNormal) * hitNormal;
            transform.position += moveVector;
        }
    }


}
