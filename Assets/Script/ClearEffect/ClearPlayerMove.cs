using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerMove : MonoBehaviour
{
    // ˆÚ“®—Ê
    [SerializeField] private float velocity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 20)
        {
            transform.position += new Vector3(velocity, 0, 0);
        }

    }
}