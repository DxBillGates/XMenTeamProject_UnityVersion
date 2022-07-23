using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraControl : MonoBehaviour
{
    [SerializeField] GameObject cameraObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = cameraObj.transform.position;
        gameObject.transform.rotation = cameraObj.transform.rotation;
        gameObject.transform.localScale = cameraObj.transform.localScale;
    }
}
