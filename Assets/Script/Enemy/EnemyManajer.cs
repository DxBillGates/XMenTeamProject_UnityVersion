using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManajer : MonoBehaviour
{
    //�v���n�u
    [SerializeField] private GameObject BarrierEnemy;
    [SerializeField] private GameObject FollowEnemy;

    //�ŏ��ɏo����
    [SerializeField] private int MaxBarrierEnemyNum = 1;
    [SerializeField] private int MaxFollowEnemyNum = 1;
    // Start is called before the first frame update
    void Start()
    {
        //�o���A�G�l�~�[����
        for (int i = 0; i < MaxBarrierEnemyNum; ++i)
        {
            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX
            Instantiate(BarrierEnemy, new Vector3(), Quaternion.identity);
        }
        //�ǂ������G�l�~�[����
        for (int i = 0; i < MaxFollowEnemyNum; ++i)
        {
            //�����̐^�񒆕ύX�Ő�������|�W�V�����ύX
            Instantiate(FollowEnemy, new Vector3(), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
