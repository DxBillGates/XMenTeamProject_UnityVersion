using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShadow : MonoBehaviour
{
    //�e�𗎂Ƃ��I�u�W�F�N�g
    [SerializeField] List<GameObject> castSadowObjects;
    //�e�̃v���n�u
    [SerializeField] GameObject shadowPrefab;
    //�e�̃N���[��
    List<GameObject> shadowClone;
    
    // Start is called before the first frame update
    void Start()
    {
        shadowClone = new List<GameObject>();

        for (int i = 0; i < castSadowObjects.Count; i++)
        {
            shadowClone.Add(Instantiate(shadowPrefab, Vector3.zero, new Quaternion(0, 1, 0, 0)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < castSadowObjects.Count; i++)
        {
            shadowClone[i].transform.position = new Vector3(castSadowObjects[i].transform.position.x, 0, castSadowObjects[i].transform.position.z);
        }
    }

    public void AddObject(GameObject obj)
    {

        castSadowObjects.Add(obj);
        shadowClone.Add(Instantiate(shadowPrefab, Vector3.zero, new Quaternion(0, 1, 0, 0)));

    }

}
