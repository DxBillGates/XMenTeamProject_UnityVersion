using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeGenerator : MonoBehaviour
{
    [SerializeField] GameObject domePrefab;

    private GameObject createdDomeObject;

    public void CreateDome(Vector3 pos,float scale)
    {
        createdDomeObject = Instantiate(domePrefab);
        createdDomeObject.transform.position = pos;
        createdDomeObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void DestroyDome()
    {
        if(createdDomeObject)
        {
            Destroy(createdDomeObject);
        }
    }
}
