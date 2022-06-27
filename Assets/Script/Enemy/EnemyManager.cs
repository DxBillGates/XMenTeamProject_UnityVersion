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
    [SerializeField] GameObject nextSceneGameClear;

    private static int aliveCount = 0;

    private GameObject[] Enemys;

    // Start is called before the first frame update
    void Start()
    {
        //�o���A�G�l�~�[����
        for (int i = 0; i < MaxBarrierEnemyNum; ++i)
        {
            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX

            // �����_���ȃ|�W�V�����̐ݒ�
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // �o���֎~�͈͓��Ȃ�ʒu��ύX
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

                }
                else break;

            }

            Instantiate(BarrierEnemy, spawnPos, Quaternion.identity);
            //���������₷
            aliveCount++;
        }
        //�ǂ������G�l�~�[����
        for (int i = 0; i < MaxFollowEnemyNum; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // �o���֎~�͈͓��Ȃ�ʒu��ύX
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX
            Instantiate(FollowEnemy, spawnPos, Quaternion.identity);
            //���������₷
            aliveCount++;
        }

        //��]�G�l�~�[����
        for (int i = 0; i < MaxRollEnemyNum; ++i)
        {
            Vector3 spawnPos =
                new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));

            while (true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                // �o���֎~�͈͓��Ȃ�ʒu��ύX
                if (Vector3.Distance(player.transform.position, spawnPos) <= Cant_Spawn_Distance)
                {
                    spawnPos =
                        new Vector3(Random.Range(-1 * Spawn_Range, Spawn_Range), 0, Random.Range(-1 * Spawn_Range, Spawn_Range));


                }
                else break;

            }

            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX
            Instantiate(RollEnemy, spawnPos, Quaternion.identity);
            //���������₷
            aliveCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //�G���S�ł�����N���A�V�[����
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
