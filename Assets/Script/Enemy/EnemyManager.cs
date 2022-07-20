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
    [SerializeField] NextScene sceneChange;

    //丸影のスクリプト取得用
    [SerializeField] GameObject circleShadowScriptObject;


    private static int aliveCount = 0;

    private GameObject[] Enemys;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //バリアエネミー生成
        CreateEnemies(BarrierEnemy, player, MaxBarrierEnemyNum);

        //追っかけエネミー生成
        CreateEnemies(FollowEnemy, player, MaxFollowEnemyNum);

        //回転エネミー生成
        CreateEnemies(RollEnemy, player, MaxRollEnemyNum);
    }

    // Update is called once per frame
    void Update()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //敵が全滅したらクリアシーンへ
        if (Enemys.Length <= 0)
        {
            sceneChange.nextSceneName = "GameClearScene";
            sceneChange.gameObject.SetActive(true);
        }
    }

    public static void DecrementAliveCount()
    {
        aliveCount--;
    }

    // 指定したプレハブの敵を指定数生成する
    public void CreateEnemies(GameObject prefab,GameObject player,int enemyAmount)
    {
        //回転エネミー生成
        for (int i = 0; i < enemyAmount; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                // 出現禁止範囲内なら位置を変更
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //引数の真ん中変更で生成するポジション変更
            circleShadowScriptObject.GetComponent<CircleShadow>().AddObject(Instantiate(prefab, spawnPos, Quaternion.identity));
            //Instantiate(prefab, spawnPos, Quaternion.identity);
            //生成数増やす
            aliveCount++;
        }
    }
}
