using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerMove : MonoBehaviour
{
    // ˆÚ“®—Ê
    [SerializeField] private float velocity = 0.1f;

    // Start is called before the first frame update
    bool isEffectEnd = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(velocity, 0, 0);

        if (transform.position.x >= 5)
        {
            isEffectEnd = true;
        }

    }

    public bool IsEffectEnd
    {
        get { return isEffectEnd; }
    }
}