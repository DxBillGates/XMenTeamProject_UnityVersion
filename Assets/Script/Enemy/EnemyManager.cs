using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレハブ
    [SerializeField] private GameObject BarrierEnemy;
    [SerializeField] private GameObject FollowEnemy;
    [SerializeField] private GameObject RollEnemy;

    //最初に出す数
    [SerializeField] private int MaxBarrierEnemyNum = 1;
    [SerializeField] private int MaxFollowEnemyNum = 1;
    [SerializeField] private int MaxRollEnemyNum = 1;

    //　敵を出現させる範囲
    [SerializeField] private float Spawn_Range = 30;
    //　プレイヤーからどれくらいの範囲に敵が出現できないか
    [SerializeField] private float Cant_Spawn_Distance = 10;

    //クリアシーン遷移用
    [SerializeField] GameObject nextSceneGameClear;

    private static int aliveCount = 0;

    private GameObject[] Enemys;

    // Start is called before the first frame update
    void Start()
    {
        //バリアエネミー生成
        for (int i = 0; i < MaxBarrierEnemyNum; ++i)
        {
            //引数の真ん中変更で生成するポジション変更

            // ランダムなポジションの設定
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // 出現禁止範囲内なら位置を変更
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

                }
                else break;

            }

            Instantiate(BarrierEnemy, spawnPos, Quaternion.identity);
            //生成数増やす
            aliveCount++;
        }
        //追っかけエネミー生成
        for (int i = 0; i < MaxFollowEnemyNum; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // 出現禁止範囲内なら位置を変更
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //引数の真ん中変更で生成するポジション変更
            Instantiate(FollowEnemy, spawnPos, Quaternion.identity);
            //生成数増やす
            aliveCount++;
        }

        //回転エネミー生成
        for (int i = 0; i < MaxRollEnemyNum; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // 出現禁止範囲内なら位置を変更
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //引数の真ん中変更で生成するポジション変更
            Instantiate(RollEnemy, spawnPos, Quaternion.identity);
            //生成数増やす
            aliveCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //敵が全滅したらクリアシーンへ
        if (Enemys.Length <= 0)
        {
            nextSceneGameClear.SetActive(true);
        }
    }

    public static void DecrementAliveCount()
    {
        aliveCount--;
    }
}
