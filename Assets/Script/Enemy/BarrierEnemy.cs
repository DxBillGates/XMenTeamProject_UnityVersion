using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemy : Enemy
{
    // プレイヤーとの開ける距離
    [SerializeField] private float playerToDistance = 10;
    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //　基本的な行動
    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);

        Vector3 moveVector = targetObject.transform.position - transform.position;

        // 設定した距離より遠ければ近づく 近ければ離れる
        if (distance >= playerToDistance)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, targetObject.transform.position, moveSpeed);

        }
        else if (distance < playerToDistance - 1)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, -1 * moveVector + transform.position, moveSpeed);

        }
    }
}
