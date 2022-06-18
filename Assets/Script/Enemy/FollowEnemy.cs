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
