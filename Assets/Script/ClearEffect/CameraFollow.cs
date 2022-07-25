using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // カメラが追従するオブジェクトを指定
    [SerializeField]protected GameObject followObject;

    // 動かすカメラ
    [SerializeField] protected Camera camera;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = camera.transform.position - followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = followObject.transform.position + offset;
    }
}
