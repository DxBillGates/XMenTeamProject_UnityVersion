using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //�v���n�u
    [SerializeField] private GameObject BarrierEnemy;
    [SerializeField] private GameObject FollowEnemy;
    [SerializeField] private GameObject RollEnemy;

    //�ŏ��ɏo����
    [SerializeField] private int MaxBarrierEnemyNum = 1;
    [SerializeField] private int MaxFollowEnemyNum = 1;
    [SerializeField] private int MaxRollEnemyNum = 1;

    //�@�G���o��������͈�
    [SerializeField] private float Spawn_Range = 30;
    //�@�v���C���[����ǂꂭ�炢�͈̔͂ɓG���o���ł��Ȃ���
    [SerializeField] private float Cant_Spawn_Distance = 10;

    //�N���A�V�[���J�ڗp
    [SerializeField] NextScene sceneChange;

    //�ۉe�̃X�N���v�g�擾�p
    [SerializeField] GameObject circleShadowScriptObject;


    private static int aliveCount = 0;

    private GameObject[] Enemys;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //�o���A�G�l�~�[����
        CreateEnemies(BarrierEnemy, player, MaxBarrierEnemyNum);

        //�ǂ������G�l�~�[����
        CreateEnemies(FollowEnemy, player, MaxFollowEnemyNum);

        //��]�G�l�~�[����
        CreateEnemies(RollEnemy, player, MaxRollEnemyNum);
    }

    // Update is called once per frame
    void Update()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //�G���S�ł�����N���A�V�[����
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

    // �w�肵���v���n�u�̓G���w�萔��������
    public void CreateEnemies(GameObject prefab,GameObject player,int enemyAmount)
    {
        //��]�G�l�~�[����
        for (int i = 0; i < enemyAmount; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                // �o���֎~�͈͓��Ȃ�ʒu��ύX
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX
            circleShadowScriptObject.GetComponent<CircleShadow>().AddObject(Instantiate(prefab, spawnPos, Quaternion.identity));
            //Instantiate(prefab, spawnPos, Quaternion.identity);
            //���������₷
            aliveCount++;
        }
    }
}
