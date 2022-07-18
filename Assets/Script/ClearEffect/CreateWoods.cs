using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < CreateWoodNum; i++)
        {
            // �Ƃ肠���������̓x�^�ł��@���������J�������ꂳ���鋗���������l
            Vector3 position =
                new Vector3(Random.Range(-100, 100), 0, Random.Range(DistanceCameraToWood, DistanceCameraToWood + 20));


            var wood = Instantiate(woodObject, position, Quaternion.identity);

            wood.transform.localScale *= Random.Range(CreateWoodMinSize, CreateWoodMaxSize);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
