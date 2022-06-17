using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemyCollision : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        // ボールに当たったときの処理
        if (collision.gameObject.tag == "Ball")
        {
            KnockBack(collision.gameObject.transform.position, collision);
        }

        if(collision.gameObject.tag == "Wall")
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
