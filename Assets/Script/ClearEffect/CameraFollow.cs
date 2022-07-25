using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // �J�������Ǐ]����I�u�W�F�N�g���w��
    [SerializeField]protected GameObject followObject;

    // �������J����
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
