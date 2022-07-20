using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShadow : MonoBehaviour
{
    //�e�𗎂Ƃ��I�u�W�F�N�g
    [SerializeField] List<GameObject> castSadowObjects;
    //�e�̃v���n�u
    [SerializeField] GameObject shadowPrefab;
    //�e�̑傫��
    [SerializeField] float Scale;
    //�e�̃N���[��
    List<GameObject> shadowClone;

    private void Awake()
    {
        castSadowObjects = new List<GameObject>();
        shadowClone = new List<GameObject>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < castSadowObjects.Count; i++)
        {
            if(castSadowObjects[i]!=null)
            {
            shadowClone[i].transform.position = new Vector3(castSadowObjects[i].transform.position.x, gameObject.transform.position.y+0.01f, castSadowObjects[i].transform.position.z);
            shadowClone[i].transform.localScale = new Vector3(Scale, Scale, Scale);

            }
            else
            {
                castSadowObjects.RemoveAt(i);
                Destroy(shadowClone[i]);
                shadowClone.RemoveAt(i);
            }
        }
    }

    public void AddObject(GameObject obj)
    {

        castSadowObjects.Add(obj);
        shadowClone.Add(Instantiate(shadowPrefab));

    }

    public void DestroyObject(GameObject obj)
    {
        castSadowObjects.Remove(obj);
    }

}
