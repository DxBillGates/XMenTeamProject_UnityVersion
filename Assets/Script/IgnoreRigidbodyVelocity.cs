using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rigidbody��velocity�𖳌�������R���|�[�l���g
public class IgnoreRigidbodyVelocity : MonoBehaviour
{
    private Rigidbody attachedRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        attachedRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        attachedRigidbody.velocity = new Vector3();
    }
}
