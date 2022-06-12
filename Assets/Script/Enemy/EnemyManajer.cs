using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManajer : MonoBehaviour
{
    //プレハブ
    [SerializeField] private GameObject BarrierEnemy;
    [SerializeField] private GameObject FollowEnemy;

    //最初に出す数
    [SerializeField] private int MaxBarrierEnemyNum = 1;
    [SerializeField] private int MaxFollowEnemyNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        //バリアエネミー生成
        for (int i = 0; i < MaxBarrierEnemyNum; ++i)
        {
            //引数の真ん中変更で生成するポジション変更
            Instantiate(BarrierEnemy, new Vector3(), Quaternion.identity);
        }
        //追っかけエネミー生成
        for (int i = 0; i < MaxFollowEnemyNum; ++i)
        {
            //引数の真ん中変更で生成するポジション変更
            Instantiate(FollowEnemy, new Vector3(), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
