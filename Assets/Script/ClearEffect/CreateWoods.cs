using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateWoods : MonoBehaviour
{
    [SerializeField] private GameObject woodObject;

    // 出現する木の本数
    [SerializeField] private int CreateWoodNum = 20;
    // 最大サイズ
    [SerializeField] private float CreateWoodMaxSize = 2;
    // 最小サイズ
    [SerializeField] private float CreateWoodMinSize = 0.1f;

    // カメラからどれだけ離れた位置に出現させるか
    [SerializeField] private float DistanceCameraToWood = 10.0f;

    private GameObject[] woods;
    

    // Start is called before the first frame update
    void Start()
    {
        // 配列の上限を変更
        Array.Resize(ref woods, CreateWoodNum);
        
        

        for (int i = 0; i < CreateWoodNum; i++)
        {
            // とりあえず横幅はベタ打ち　かつｚ軸をカメラ離れさせる距離から特定値
            Vector3 position =
                new Vector3(UnityEngine.Random.Range(-100, 100), 0, 
                UnityEngine.Random.Range(DistanceCameraToWood, DistanceCameraToWood + 20));


            var wood = Instantiate(woodObject, position, Quaternion.identity);

            wood.transform.localScale *= UnityEngine.Random.Range(CreateWoodMinSize, CreateWoodMaxSize);

            // スクロール用にリストに追加
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
