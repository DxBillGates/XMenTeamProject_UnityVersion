using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierControl : MonoBehaviour
{
    Vector3 target { get; set; }
    Vector3 move { get; set; }
    bool isOpen { get; set; }
    [SerializeField] private GameObject barrierObject;
    private GameObject barrieClone;
    //���a
    [SerializeField] private float radius;
    //����W�J���钷��
    [SerializeField] private int openSpan;
    //�W�J���Ă��钷��
    int time;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        //�W�J��
        if (time < openSpan)
        {
            time++;
        }
        //�W�J���ĂȂ���
        else
        {
            if (isOpen)
            {
                isOpen = false;
                Destroy(barrieClone);
                time = 0;
            }
        }
        //�̂̌�����move���狁�߂�
        float angle = Mathf.Atan2(move.x, move.z);
        Vector3 
        //�o���A��W�J������W
        if (Input.GetKeyDown(KeyCode.Space))
        {
            barrieClone = Instantiate(barrierObject, new Vector3(-5.0f, 0.0f, 0.0f), new Quaternion(0, 1, 0, angle));
        }

        barrieClone.transform.rotation = new Quaternion(0, 1, 0, angle);
        barrieClone. transform.position = );
    }
}
