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

    //player入れる
    protected GameObject targetObject;


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
        transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, moveSpeed);
    }


    /// <summary>
    /// 何かに当たったときのノックバック処理
    /// </summary>
    /// <param name="">当たった敵の位置</param>
    protected void KnockBack(Vector3 hitPos)
    {
        // ノックバックする位置を決める
        Vector3 moveVector = -1 * (hitPos - transform.position);
        // 正規化させる
        moveVector = knock_back_speed * moveVector.normalized;

        transform.position =
            Vector3.MoveTowards(transform.position, moveVector + transform.position, knock_back_speed);
    }

    private void OnTriggerStay(Collider collision)
    {

        // ボールに当たったときの処理
        if (collision.gameObject.name == "Ball")
        {
            KnockBack(collision.transform.position);
        }

    }

}
