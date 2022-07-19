using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemyCollision : MonoBehaviour
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
        //親オブジェクト取得
        GameObject enemyParent = transform.root.gameObject;
        BarrierEnemy barrierEnemy = enemyParent.GetComponent<BarrierEnemy>();

        if (collision.gameObject.tag == "Wall")
        {
            //親で親のトランスフォーム修正
            barrierEnemy.WallCollsion(collision.transform);
        }

        if (collision.gameObject.CompareTag("Pin"))
        {
            barrierEnemy.PinCollision(collision.transform);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //親オブジェクト取得
        GameObject enemyParent = transform.root.gameObject;
        BarrierEnemy barrierEnemy = enemyParent.GetComponent<BarrierEnemy>();

        // ボールに当たったときの処理
        Ball ballComponent;
        if (collision.gameObject.CompareTag("Ball"))
        {
            ballComponent = collision.gameObject.GetComponent<Ball>();
            if (ballComponent.GetSpeed() > 0)
            {
                barrierEnemy.HitBall = ballComponent;

                //親で判定
                barrierEnemy.KnockBack(collision);
            }
        }
    }
}
