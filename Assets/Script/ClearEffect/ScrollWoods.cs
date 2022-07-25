using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWoods : MonoBehaviour
{
    [SerializeField] protected Camera camera;

    [SerializeField] protected GameObject ground;
    [SerializeField] protected GameObject groundSecond;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var woods = GetComponent<CreateWoods>().Woods;

        // �؂̃X�N���[���p
        foreach(GameObject a in woods)
        {
            // ������ȏ�J�������痣�ꂽ��ʒu��ύX
            if (camera.transform.position.x - a.transform.position.x > 150)
            {
                Vector3 pos = a.transform.position;

                a.transform.position = new Vector3(camera.transform.position.x + 30, pos.y, pos.z);
            }

        }

        // �n�ʂ̃X�N���[���p
        if (camera.transform.position.x - groundSecond.transform.position.x > 450)
        {
            groundSecond.transform.position =
                new Vector3(ground.transform.position.x + 390, 0, groundSecond.transform.position.z);
        }

        if (camera.transform.position.x - ground.transform.position.x > 450)
        {
            ground.transform.position =
                new Vector3(groundSecond.transform.position.x + 390, 0, ground.transform.position.z);
        }

    }
}
