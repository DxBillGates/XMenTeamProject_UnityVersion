using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < CreateWoodNum; i++)
        {
            // とりあえず横幅はベタ打ち　かつｚ軸をカメラ離れさせる距離から特定値
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
