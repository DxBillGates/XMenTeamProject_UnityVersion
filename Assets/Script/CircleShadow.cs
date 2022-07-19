using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShadow : MonoBehaviour
{
    //影を落とすオブジェクト
    [SerializeField] List<GameObject> castSadowObjects;
    //影のプレハブ
    [SerializeField] GameObject shadowPrefab;
    //影の大きさ
    [SerializeField] float Scale;
    //影のクローン
    List<GameObject> shadowClone;

    
    // Start is called before the first frame update
    void Start()
    {
        shadowClone = new List<GameObject>();

        for (int i = 0; i < castSadowObjects.Count; i++)
        {
            shadowClone.Add(Instantiate(shadowPrefab));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < castSadowObjects.Count; i++)
        {
            shadowClone[i].transform.position = new Vector3(castSadowObjects[i].transform.position.x, gameObject.transform.position.y+0.01f, castSadowObjects[i].transform.position.z);
            shadowClone[i].transform.localScale = new Vector3(Scale, Scale, Scale);
        }
    }

    public void AddObject(GameObject obj)
    {

        castSadowObjects.Add(obj);
        shadowClone.Add(Instantiate(shadowPrefab, Vector3.zero, new Quaternion(0, 1, 0, 0)));

    }

}
