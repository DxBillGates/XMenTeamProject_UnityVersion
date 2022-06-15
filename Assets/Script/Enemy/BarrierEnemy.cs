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

    private Vector3[] ballDir;

    // 向き変更用
    private int ballCurrentNum = 0; // ボールの情報を入れる番号
    private int ballBeforeNum = 0; // 向き変更用の番号
    private bool firstCountflg = false;// 最初、配列を入れるまでの関数

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

        ChangePose();

        Move();
    }

    //　基本的な行動
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        Vector3 moveVector = targetObject.transform.position - transform.position;
        moveVector.Normalize();



        // 設定した距離より遠ければ近づく 近ければ離れる
        if (distance >= playerToDistance)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, moveVector + transform.position, moveSpeed);

        }
        else if (distance < playerToDistance - 2)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, -1 * moveVector + transform.position, moveSpeed);

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


   

}
