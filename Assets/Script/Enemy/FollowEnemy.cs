using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");

        // 念のためタグ付け
        transform.tag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFollow();

        ChangePose();
    }

    private void ChangePose()
    {
        // プレイヤーの方向ベクトルを取得し、それを使い回転させる
        Vector3 playerV = targetObject.transform.position - transform.position;

        if(playerV != Vector3.zero)transform.rotation = Quaternion.LookRotation(playerV);
    }

    private void OnTriggerStay(Collider collision)
    {
        // ボールに当たったときの処理
        if (collision.gameObject.name == "Ball")
        {
            KnockBack(collision.gameObject.transform.position,collision);
        }
    }
}
