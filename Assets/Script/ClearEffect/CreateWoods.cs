using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateWoods : MonoBehaviour
{
    [SerializeField] private GameObject woodObject;

    // �o������؂̖{��
    [SerializeField] private int CreateWoodNum = 20;
    // �ő�T�C�Y
    [SerializeField] private float CreateWoodMaxSize = 2;
    // �ŏ��T�C�Y
    [SerializeField] private float CreateWoodMinSize = 0.1f;

    // �J��������ǂꂾ�����ꂽ�ʒu�ɏo�������邩
    [SerializeField] private float DistanceCameraToWood = 10.0f;

    private GameObject[] woods;
    

    // Start is called before the first frame update
    void Start()
    {
        // �z��̏����ύX
        Array.Resize(ref woods, CreateWoodNum);
        
        

        for (int i = 0; i < CreateWoodNum; i++)
        {
            // �Ƃ肠���������̓x�^�ł��@���������J�������ꂳ���鋗���������l
            Vector3 position =
                new Vector3(UnityEngine.Random.Range(-100, 100), 0, 
                UnityEngine.Random.Range(DistanceCameraToWood, DistanceCameraToWood + 20));


            var wood = Instantiate(woodObject, position, Quaternion.identity);

            wood.transform.localScale *= UnityEngine.Random.Range(CreateWoodMinSize, CreateWoodMaxSize);

            // �X�N���[���p�Ƀ��X�g�ɒǉ�
            woods[i] = wood;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject[] Woods
    {
        get { return woods; }
    }
}
