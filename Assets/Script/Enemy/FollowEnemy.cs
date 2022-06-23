using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        GameObject SEPlayManager = GameObject.FindGameObjectWithTag("SEPlayManager");
        sePlayManagerComponent = SEPlayManager.GetComponent<SEPlayManager>();

        // animetor
        GameObject temp = transform.root.Find("EnemyModel").gameObject;
        animator = temp.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFollow();

        // ヒットストップ時にモデルのアニメーションもストップさせる
        animator.SetFloat("Speed",GameTimeManager.GetInstance().GetTime());
        //ChangePose();
    }

    private void ChangePose()
    {
        // プレイヤーの方向ベクトルを取得し、それを使い回転させる
        Vector3 playerV = targetObject.transform.position - transform.position;
        playerV.y = 0;

        if (playerV != Vector3.zero) transform.rotation = Quaternion.LookRotation(playerV);
    }

    private void OnTriggerStay(Collider collision)
    {
        // ボールに当たったときの処理
        if (collision.gameObject.tag == "Ball" && collision.GetComponent<Ball>().state != BallState.HOLD_PLAYER)
        {
            KnockBack(collision);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            WallCollsion(collision.transform);
        }

    }
}
